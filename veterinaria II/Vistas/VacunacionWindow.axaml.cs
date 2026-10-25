using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Data.Sqlite;
using VeterinariaV2.Datos;
using VeterinariaV2.Modelos;

namespace VeterinariaV2.Vistas;

public partial class VacunacionWindow : Window
{
    private readonly VacunacionRepositorio _repo = new();
    private List<Mascota> _mascotas = new();
    private Vacunacion? _seleccionada;

    public VacunacionWindow()
    {
        InitializeComponent();
        _mascotas = new MascotaRepositorio().Listar();
        cmbMascota.ItemsSource = _mascotas;
        Cargar();

        if (_mascotas.Count == 0)
            lblMensaje.Text = "Primero registra al menos una mascota.";
    }

    private void Cargar()
    {
        lstVacunas.ItemsSource = _repo.Listar();
    }

    private void Limpiar()
    {
        cmbMascota.SelectedItem = null;
        txtVacuna.Text = "";
        dpAplicacion.SelectedDate = null;
        dpProxima.SelectedDate = null;
        chkSinProxima.IsChecked = false;
        lblMensaje.Text = "";
        _seleccionada = null;
        lstVacunas.SelectedItem = null;
    }

    private void LstVacunas_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        _seleccionada = lstVacunas.SelectedItem as Vacunacion;
        if (_seleccionada == null) return;

        cmbMascota.SelectedItem = _mascotas.Find(m => m.IdMascota == _seleccionada.IdMascota);
        txtVacuna.Text = _seleccionada.Vacuna;

        dpAplicacion.SelectedDate = DateTime.TryParse(_seleccionada.FechaAplicacion, out var fa)
            ? new DateTimeOffset(fa) : (DateTimeOffset?)null;

        if (DateTime.TryParse(_seleccionada.ProximaDosis, out var fp))
        {
            dpProxima.SelectedDate = new DateTimeOffset(fp);
            chkSinProxima.IsChecked = false;
        }
        else
        {
            dpProxima.SelectedDate = null;
            chkSinProxima.IsChecked = true;
        }
        lblMensaje.Text = "";
    }

    private void BtnNuevo_Click(object? sender, RoutedEventArgs e)
    {
        Limpiar();
    }

    private void BtnGuardar_Click(object? sender, RoutedEventArgs e)
    {
        var mascota = cmbMascota.SelectedItem as Mascota;
        if (mascota == null)
        {
            lblMensaje.Text = "Selecciona la mascota.";
            return;
        }
        if (string.IsNullOrWhiteSpace(txtVacuna.Text))
        {
            lblMensaje.Text = "El nombre de la vacuna es obligatorio.";
            return;
        }
        if (dpAplicacion.SelectedDate == null)
        {
            lblMensaje.Text = "Selecciona la fecha de aplicación.";
            return;
        }

        string proxima = "";
        if (chkSinProxima.IsChecked != true && dpProxima.SelectedDate != null)
        {
            if (dpProxima.SelectedDate.Value.Date < dpAplicacion.SelectedDate.Value.Date)
            {
                lblMensaje.Text = "La próxima dosis no puede ser anterior a la aplicación.";
                return;
            }
            proxima = dpProxima.SelectedDate.Value.ToString("yyyy-MM-dd");
        }

        var v = _seleccionada ?? new Vacunacion();
        v.IdMascota = mascota.IdMascota;
        v.Vacuna = txtVacuna.Text!.Trim();
        v.FechaAplicacion = dpAplicacion.SelectedDate.Value.ToString("yyyy-MM-dd");
        v.ProximaDosis = proxima;

        if (_seleccionada == null) _repo.Crear(v);
        else _repo.Actualizar(v);

        Limpiar();
        Cargar();
        lblMensaje.Text = "Guardado correctamente.";
    }

    private void BtnEliminar_Click(object? sender, RoutedEventArgs e)
    {
        if (_seleccionada == null)
        {
            lblMensaje.Text = "Selecciona una vacuna de la lista.";
            return;
        }

        try
        {
            _repo.Eliminar(_seleccionada.IdVacunacion);
            Limpiar();
            Cargar();
            lblMensaje.Text = "Eliminada correctamente.";
        }
        catch (SqliteException)
        {
            lblMensaje.Text = "No se pudo eliminar el registro.";
        }
    }
}
