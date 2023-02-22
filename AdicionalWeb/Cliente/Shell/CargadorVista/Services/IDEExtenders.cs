// Archivo: Extenders.cs
using System.Windows.Forms;

public static class IDEExtenders
{
    /// <summary>
    /// Metodo con el que se evita excepciones con el CrossThreading de los controles WinForm
    /// </summary>
    /// <param name="this">Es el Control afectado NO SE UTILIZA EL PARAMETRO SE PASA AUTOMATIVAMENTE</param>
    /// <param name="code">Es el delegado que realizara la acción a realizar</param>
    /// <example> this.txtNombre.SetSafeThread(delegate { this.txtNombre.Text = "Fulanito"; }); </example>
    public static void BeginSafe(this Control @this, MethodInvoker code)
    {
        if (@this.InvokeRequired)
        {
            @this.BeginInvoke(code);
            return;
        }
        code.Invoke();
    }

    public static void BeginThreadSafe(this Control @this, MethodInvoker code)
    {
        code.BeginInvoke((async) =>
            {
                @this.BeginSafe(code);
            }, null);
    }
}
