using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Data.Sqlite;
using VeterinariaV2.Datos;
using VeterinariaV2.Modelos;

namespace VeterinariaV2.Vistas;

public partial class UsuariosWindow : Window
{
    private readonly UsuarioRepositorio _repo = new();
    private Usuario? _seleccionado;

    public UsuariosWindow()
    {
        InitializeComponent();
        cmbRol.ItemsSource = new[] { "Administrador", "Recepcionista", "Veterinario" };
        Cargar();
    }

    private void Cargar()
    {
        lstUsuarios.ItemsSource = _repo.Listar();
    }

    private void Limpiar()
    {
        txtNombreCompleto.Text = "";
        txtNombreUsuario.Text = "";
        txtClave.Text = "";
        cmbRol.SelectedItem = null;
        chkActivo.IsChecked = true;
        lblMensaje.Text = "";
        _seleccionado = null;
        lstUsuarios.SelectedItem = null;
    }

    private bool EsMiCuenta(Usuario? u) =>
        u != null && Sesion.Actual != null && u.IdUsuario == Sesion.Actual.IdUsuario;

    private void LstUsuarios_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        _seleccionado = lstUsuarios.SelectedItem as Usuario;
        if (_seleccionado == null) return;

        txtNombreCompleto.Text = _seleccionado.NombreCompleto;
        txtNombreUsuario.Text = _seleccionado.NombreUsuario;
        txtClave.Text = "";
        cmbRol.SelectedItem = _seleccionado.Rol;
        chkActivo.IsChecked = _seleccionado.Activo;
        lblMensaje.Text = "";
    }

    private void BtnNuevo_Click(object? sender, RoutedEventArgs e)
    {
        Limpiar();
    }

    private void BtnGuardar_Click(object? sender, RoutedEventArgs e)
    {
        var rol = cmbRol.SelectedItem as string;
        if (string.IsNullOrWhiteSpace(txtNombreCompleto.Text) ||
            string.IsNullOrWhiteSpace(txtNombreUsuario.Text) || rol == null)
        {
            lblMensaje.Text = "Nombre completo, usuario y rol son obligatorios.";
            return;
        }
        if (_seleccionado == null && string.IsNullOrEmpty(txtClave.Text))
        {
            lblMensaje.Text = "La contraseña es obligatoria para un usuario nuevo.";
            return;
        }
        if (EsMiCuenta(_seleccionado) && (rol != "Administrador" || chkActivo.IsChecked != true))
        {
            lblMensaje.Text = "No puedes quitarte el rol de administrador ni desactivar tu propia cuenta.";
            return;
        }

        var u = _seleccionado ?? new Usuario();
        u.NombreCompleto = txtNombreCompleto.Text!.Trim();
        u.NombreUsuario = txtNombreUsuario.Text!.Trim();
        u.Rol = rol;
        u.Activo = chkActivo.IsChecked == true;

        try
        {
            if (_seleccionado == null) _repo.Crear(u, txtClave.Text!);
            else _repo.Actualizar(u, txtClave.Text);
        }
        catch (SqliteException)
        {
            lblMensaje.Text = "No se pudo guardar. Verifica que el nombre de usuario no esté repetido.";
            return;
        }

        Limpiar();
        Cargar();
        lblMensaje.Text = "Guardado correctamente.";
    }

    private void BtnEliminar_Click(object? sender, RoutedEventArgs e)
    {
        if (_seleccionado == null)
        {
            lblMensaje.Text = "Selecciona un usuario de la lista.";
            return;
        }
        if (EsMiCuenta(_seleccionado))
        {
            lblMensaje.Text = "No puedes eliminar tu propia cuenta.";
            return;
        }

        try
        {
            _repo.Eliminar(_seleccionado.IdUsuario);
            Limpiar();
            Cargar();
            lblMensaje.Text = "Eliminado correctamente.";
        }
        catch (SqliteException)
        {
            lblMensaje.Text = "No se puede eliminar: el usuario tiene citas asignadas. Desactívalo en su lugar.";
        }
    }
}
