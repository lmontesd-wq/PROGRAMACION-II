using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Data.Sqlite;
using VeterinariaV2.Datos;
using VeterinariaV2.Modelos;

namespace VeterinariaV2.Vistas;

public partial class MascotasWindow : Window
{
    private readonly MascotaRepositorio _repo = new();
    private readonly PropietarioRepositorio _repoPropietarios = new();
    private List<Propietario> _propietarios = new();
    private Mascota? _seleccionada;

    public MascotasWindow()
    {
        InitializeComponent();
        cmbSexo.ItemsSource = new[] { "Macho", "Hembra" };
        _propietarios = _repoPropietarios.Listar();
        cmbPropietario.ItemsSource = _propietarios;
        Cargar();

        if (_propietarios.Count == 0)
            lblMensaje.Text = "Primero registra al menos un propietario.";
    }

    private void Cargar()
    {
        lstMascotas.ItemsSource = _repo.Listar();
    }

    private void Limpiar()
    {
        cmbPropietario.SelectedItem = null;
        txtNombre.Text = "";
        txtEspecie.Text = "";
        txtRaza.Text = "";
        cmbSexo.SelectedItem = null;
        dpNacimiento.SelectedDate = null;
        lblMensaje.Text = "";
        _seleccionada = null;
        lstMascotas.SelectedItem = null;
    }

    private void LstMascotas_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        _seleccionada = lstMascotas.SelectedItem as Mascota;
        if (_seleccionada == null) return;

        cmbPropietario.SelectedItem = _propietarios.Find(p => p.IdPropietario == _seleccionada.IdPropietario);
        txtNombre.Text = _seleccionada.Nombre;
        txtEspecie.Text = _seleccionada.Especie;
        txtRaza.Text = _seleccionada.Raza;
        cmbSexo.SelectedItem = string.IsNullOrEmpty(_seleccionada.Sexo) ? null : _seleccionada.Sexo;

        if (DateTime.TryParse(_seleccionada.FechaNacimiento, out var fecha))
            dpNacimiento.SelectedDate = new DateTimeOffset(fecha);
        else
            dpNacimiento.SelectedDate = null;

        lblMensaje.Text = "";
    }

    private void BtnNuevo_Click(object? sender, RoutedEventArgs e)
    {
        Limpiar();
    }

    private void BtnGuardar_Click(object? sender, RoutedEventArgs e)
    {
        var propietario = cmbPropietario.SelectedItem as Propietario;
        if (propietario == null)
        {
            lblMensaje.Text = "Selecciona un propietario.";
            return;
        }
        if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtEspecie.Text))
        {
            lblMensaje.Text = "El nombre y la especie son obligatorios.";
            return;
        }

        var m = _seleccionada ?? new Mascota();
        m.IdPropietario = propietario.IdPropietario;
        m.Nombre = txtNombre.Text!.Trim();
        m.Especie = txtEspecie.Text!.Trim();
        m.Raza = (txtRaza.Text ?? "").Trim();
        m.Sexo = cmbSexo.SelectedItem as string ?? "";
        m.FechaNacimiento = dpNacimiento.SelectedDate?.ToString("yyyy-MM-dd") ?? "";

        if (_seleccionada == null) _repo.Crear(m);
        else _repo.Actualizar(m);

        Limpiar();
        Cargar();
        lblMensaje.Text = "Guardado correctamente.";
    }

    private void BtnEliminar_Click(object? sender, RoutedEventArgs e)
    {
        if (_seleccionada == null)
        {
            lblMensaje.Text = "Selecciona una mascota de la lista.";
            return;
        }

        try
        {
            _repo.Eliminar(_seleccionada.IdMascota);
            Limpiar();
            Cargar();
            lblMensaje.Text = "Eliminada correctamente.";
        }
        catch (SqliteException)
        {
            lblMensaje.Text = "No se puede eliminar: la mascota tiene citas o vacunas registradas.";
        }
    }
}
