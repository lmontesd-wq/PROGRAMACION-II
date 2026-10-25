using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Data.Sqlite;
using VeterinariaV2.Datos;
using VeterinariaV2.Modelos;

namespace VeterinariaV2.Vistas;

public partial class ConsultasWindow : Window
{
    private readonly ConsultaRepositorio _repo = new();
    private readonly CitaRepositorio _repoCitas = new();
    private List<Cita> _citas = new();
    private Consulta? _seleccionada;
    private readonly int? _filtroVeterinario;

    public ConsultasWindow()
    {
        InitializeComponent();

        var u = Sesion.Actual;
        _filtroVeterinario = (u != null && u.Rol == "Veterinario") ? u.IdUsuario : (int?)null;
        lblTitulo.Text = _filtroVeterinario.HasValue ? "Mis consultas" : "Consultas registradas";

        CargarCitas();
        Cargar();

        if (_citas.Count == 0)
            lblMensaje.Text = "No hay citas disponibles para registrar consultas.";
    }

    private void CargarCitas()
    {
        _citas = _repoCitas.Listar(_filtroVeterinario)
            .Where(c => c.Estado != "Cancelada")
            .ToList();
        cmbCita.ItemsSource = _citas;
    }

    private void Cargar()
    {
        lstConsultas.ItemsSource = _repo.Listar(_filtroVeterinario);
    }

    private void Limpiar()
    {
        cmbCita.IsEnabled = true;
        cmbCita.SelectedItem = null;
        txtDiagnostico.Text = "";
        txtTratamiento.Text = "";
        txtObservaciones.Text = "";
        lblMensaje.Text = "";
        _seleccionada = null;
        lstConsultas.SelectedItem = null;
    }

    private void LstConsultas_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        _seleccionada = lstConsultas.SelectedItem as Consulta;
        if (_seleccionada == null) return;

        cmbCita.SelectedItem = _citas.Find(c => c.IdCita == _seleccionada.IdCita);
        cmbCita.IsEnabled = false;
        txtDiagnostico.Text = _seleccionada.Diagnostico;
        txtTratamiento.Text = _seleccionada.Tratamiento;
        txtObservaciones.Text = _seleccionada.Observaciones;
        lblMensaje.Text = "";
    }

    private void BtnNuevo_Click(object? sender, RoutedEventArgs e)
    {
        Limpiar();
    }

    private void BtnGuardar_Click(object? sender, RoutedEventArgs e)
    {
        var cita = cmbCita.SelectedItem as Cita;
        if (cita == null)
        {
            lblMensaje.Text = "Selecciona la cita.";
            return;
        }
        if (string.IsNullOrWhiteSpace(txtDiagnostico.Text))
        {
            lblMensaje.Text = "El diagnóstico es obligatorio.";
            return;
        }

        int idActual = _seleccionada?.IdConsulta ?? 0;
        if (_repo.ExistePorCita(cita.IdCita, idActual))
        {
            lblMensaje.Text = "Esa cita ya tiene una consulta. Selecciónala en la lista para editarla.";
            return;
        }

        var c = _seleccionada ?? new Consulta();
        c.IdCita = cita.IdCita;
        c.Diagnostico = txtDiagnostico.Text!.Trim();
        c.Tratamiento = (txtTratamiento.Text ?? "").Trim();
        c.Observaciones = (txtObservaciones.Text ?? "").Trim();

        if (_seleccionada == null) _repo.Crear(c);
        else _repo.Actualizar(c);

        Limpiar();
        CargarCitas();
        Cargar();
        lblMensaje.Text = "Guardado correctamente.";
    }

    private void BtnEliminar_Click(object? sender, RoutedEventArgs e)
    {
        if (_seleccionada == null)
        {
            lblMensaje.Text = "Selecciona una consulta de la lista.";
            return;
        }

        try
        {
            _repo.Eliminar(_seleccionada.IdConsulta);
            Limpiar();
            CargarCitas();
            Cargar();
            lblMensaje.Text = "Eliminada correctamente. La cita volvió a Pendiente.";
        }
        catch (SqliteException)
        {
            lblMensaje.Text = "No se pudo eliminar la consulta.";
        }
    }
}
