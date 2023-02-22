using System;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using EstandarCliente.Infrastructure.Interface;

namespace EstandarCliente.CargadorVistas.CargadorVistasMdl
{
    [SmartPart]
    public partial class VistaCargador
    {
        /// <summary>
        /// Sets the presenter. The dependency injection system will automatically
        /// create a new presenter for you.
        /// </summary>
        [CreateNew]
        public VistaCargadorPresenter Presenter
        {
            set
            {
                _presenter = value;
                _presenter.View = this;
            }
        }
    }
}

