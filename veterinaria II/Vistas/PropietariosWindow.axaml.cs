using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Data.Sqlite;
using VeterinariaV2.Datos;
using VeterinariaV2.Modelos;

namespace VeterinariaV2.Vistas;

public partial class PropietariosWindow : Window
{
    private readonly PropietarioRepositorio _repo = new();
    private Propietario? _seleccionado;

    public PropietariosWindow()
    {
        InitializeComponent();
        Cargar();
    }

    private void Cargar()
    {
        lstPropietarios.ItemsSource = _repo.Listar();
    }

    private void Limpiar()
    {
        txtNombre.Text = "";
        txtTelefono.Text = "";
        txtCorreo.Text = "";
        txtDireccion.Text = "";
        lblMensaje.Text = "";
        _seleccionado = null;
        lstPropietarios.SelectedItem = null;
    }

    private void LstPropietarios_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        _seleccionado = lstPropietarios.SelectedItem as Propietario;
        if (_seleccionado == null) return;
        txtNombre.Text = _seleccionado.Nombre;
        txtTelefono.Text = _seleccionado.Telefono;
        txtCorreo.Text = _seleccionado.Correo;
        txtDireccion.Text = _seleccionado.Direccion;
        lblMensaje.Text = "";
    }

    private void BtnNuevo_Click(object? sender, RoutedEventArgs e)
    {
        Limpiar();
    }

    private void BtnGuardar_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNombre.Text))
        {
            lblMensaje.Text = "El nombre es obligatorio.";
            return;
        }

        var p = _seleccionado ?? new Propietario();
        p.Nombre = txtNombre.Text!.Trim();
        p.Telefono = (txtTelefono.Text ?? "").Trim();
        p.Correo = (txtCorreo.Text ?? "").Trim();
        p.Direccion = (txtDireccion.Text ?? "").Trim();

        if (_seleccionado == null) _repo.Crear(p);
        else _repo.Actualizar(p);

        Limpiar();
        Cargar();
        lblMensaje.Text = "Guardado correctamente.";
    }

    private void BtnEliminar_Click(object? sender, RoutedEventArgs e)
    {
        if (_seleccionado == null)
        {
            lblMensaje.Text = "Selecciona un propietario de la lista.";
            return;
        }

        try
        {
            _repo.Eliminar(_seleccionado.IdPropietario);
            Limpiar();
            Cargar();
            lblMensaje.Text = "Eliminado correctamente.";
        }
        catch (SqliteException)
        {
            lblMensaje.Text = "No se puede eliminar: el propietario tiene mascotas registradas.";
        }
    }
}
