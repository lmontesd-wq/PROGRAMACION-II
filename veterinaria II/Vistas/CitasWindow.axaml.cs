using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Data.Sqlite;
using VeterinariaV2.Datos;
using VeterinariaV2.Modelos;

namespace VeterinariaV2.Vistas;

public partial class CitasWindow : Window
{
    private readonly CitaRepositorio _repo = new();
    private List<Mascota> _mascotas = new();
    private List<Usuario> _veterinarios = new();
    private Cita? _seleccionada;
    private readonly bool _soloLectura;
    private readonly int? _filtroVeterinario;

    public CitasWindow()
    {
        InitializeComponent();

        var u = Sesion.Actual;
        _soloLectura = u != null && u.Rol == "Veterinario";
        _filtroVeterinario = _soloLectura ? u!.IdUsuario : (int?)null;
        pnlFormulario.IsVisible = !_soloLectura;
        lblTitulo.Text = _soloLectura ? "Mis citas" : "Citas registradas";

        cmbEstado.ItemsSource = new[] { "Pendiente", "Atendida", "Cancelada" };
        cmbEstado.SelectedItem = "Pendiente";

        _mascotas = new MascotaRepositorio().Listar();
        cmbMascota.ItemsSource = _mascotas;

        _veterinarios = new UsuarioRepositorio().Listar()
            .Where(x => x.Rol == "Veterinario" && x.Activo)
            .ToList();
        cmbVeterinario.ItemsSource = _veterinarios;

        Cargar();

        if (!_soloLectura && (_mascotas.Count == 0 || _veterinarios.Count == 0))
            lblMensaje.Text = "Para agendar necesitas al menos una mascota y un usuario con rol Veterinario.";
    }

    private void Cargar()
    {
        lstCitas.ItemsSource = _repo.Listar(_filtroVeterinario);
    }

    private void Limpiar()
    {
        cmbMascota.SelectedItem = null;
        cmbVeterinario.SelectedItem = null;
        dpFecha.SelectedDate = null;
        tpHora.SelectedTime = null;
        txtMotivo.Text = "";
        cmbEstado.SelectedItem = "Pendiente";
        lblMensaje.Text = "";
        _seleccionada = null;
        lstCitas.SelectedItem = null;
    }

    private void LstCitas_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        _seleccionada = lstCitas.SelectedItem as Cita;
        if (_seleccionada == null || _soloLectura) return;

        cmbMascota.SelectedItem = _mascotas.Find(m => m.IdMascota == _seleccionada.IdMascota);
        cmbVeterinario.SelectedItem = _veterinarios.Find(v => v.IdUsuario == _seleccionada.IdVeterinario);

        if (DateTime.TryParse(_seleccionada.FechaHora, out var fh))
        {
            dpFecha.SelectedDate = new DateTimeOffset(fh.Date);
            tpHora.SelectedTime = fh.TimeOfDay;
        }
        else
        {
            dpFecha.SelectedDate = null;
            tpHora.SelectedTime = null;
        }

        txtMotivo.Text = _seleccionada.Motivo;
        cmbEstado.SelectedItem = _seleccionada.Estado;
        lblMensaje.Text = "";
    }

    private void BtnNuevo_Click(object? sender, RoutedEventArgs e)
    {
        Limpiar();
    }

    private void BtnGuardar_Click(object? sender, RoutedEventArgs e)
    {
        var mascota = cmbMascota.SelectedItem as Mascota;
        var vet = cmbVeterinario.SelectedItem as Usuario;
        var fecha = dpFecha.SelectedDate;
        var hora = tpHora.SelectedTime;

        if (mascota == null || vet == null)
        {
            lblMensaje.Text = "Selecciona la mascota y el veterinario.";
            return;
        }
        if (fecha == null || hora == null)
        {
            lblMensaje.Text = "Selecciona la fecha y la hora.";
            return;
        }

        string fechaHora = (fecha.Value.Date + hora.Value).ToString("yyyy-MM-dd HH:mm");
        string estado = cmbEstado.SelectedItem as string ?? "Pendiente";
        int idActual = _seleccionada?.IdCita ?? 0;

        if (estado != "Cancelada" && _repo.ExisteConflicto(vet.IdUsuario, fechaHora, idActual))
        {
            lblMensaje.Text = "Ese veterinario ya tiene una cita en esa fecha y hora.";
            return;
        }

        var c = _seleccionada ?? new Cita();
        c.IdMascota = mascota.IdMascota;
        c.IdVeterinario = vet.IdUsuario;
        c.FechaHora = fechaHora;
        c.Motivo = (txtMotivo.Text ?? "").Trim();
        c.Estado = estado;

        if (_seleccionada == null) _repo.Crear(c);
        else _repo.Actualizar(c);

        Limpiar();
        Cargar();
        lblMensaje.Text = "Guardado correctamente.";
    }

    private void BtnEliminar_Click(object? sender, RoutedEventArgs e)
    {
        if (_seleccionada == null)
        {
            lblMensaje.Text = "Selecciona una cita de la lista.";
            return;
        }

        try
        {
            _repo.Eliminar(_seleccionada.IdCita);
            Limpiar();
            Cargar();
            lblMensaje.Text = "Eliminada correctamente.";
        }
        catch (SqliteException)
        {
            lblMensaje.Text = "No se puede eliminar: la cita ya tiene una consulta registrada.";
        }
    }
}
