using Avalonia.Controls;
using Avalonia.Interactivity;
using VeterinariaV2.Datos;

namespace VeterinariaV2.Vistas;

public partial class LoginWindow : Window
{
    private readonly UsuarioRepositorio _repo = new();

    public LoginWindow()
    {
        InitializeComponent();
    }

    private void BtnEntrar_Click(object? sender, RoutedEventArgs e)
    {
        var u = _repo.Autenticar((txtUsuario.Text ?? "").Trim(), txtClave.Text ?? "");
        if (u == null)
        {
            lblMensaje.Text = "Usuario o contraseña incorrectos.";
            return;
        }

        Sesion.Actual = u;
        Close(true);
    }
}
