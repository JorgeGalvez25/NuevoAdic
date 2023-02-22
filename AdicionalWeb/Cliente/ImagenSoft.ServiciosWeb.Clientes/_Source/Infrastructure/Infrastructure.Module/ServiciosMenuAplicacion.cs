using System;
using System.Collections.Generic;
using System.Text;
using EstandarCliente.Infrastructure.Interface.Services;
using EstandarCliente.Infrastructure.Interface.Constants;
using Microsoft.Practices.CompositeUI;

namespace EstandarCliente.Infrastructure.Module
{
    public class ServiciosMenuAplicacion : IServiciosMenuAplicacion
    {

        private WorkItem m_workItem;

        public WorkItem _WorkItem
        {
            get { return m_workItem; }
            set { m_workItem = value; }
        }

        public ServiciosMenuAplicacion(WorkItem workItem)
        {
            this._WorkItem = workItem;
        }

        public void AgregaItem(string Menu, string Texto, DevExpress.XtraBars.ItemClickEventHandler Evento, System.Drawing.Image Imagen, System.Windows.Forms.Shortcut ShortCut)
        {
            DevExpress.XtraBars.BarButtonItem barItem = ObtenerBarItem(Texto, Imagen, ShortCut);
            barItem.ItemClick += Evento;

            DevExpress.XtraBars.Ribbon.RibbonControl ribbon = _WorkItem.RootWorkItem.Items[UIExtensionSiteNames.Ribbon] as DevExpress.XtraBars.Ribbon.RibbonControl;
            ribbon.Items.Add(barItem);

            DevExpress.XtraBars.BarSubItem subMenu = _WorkItem.RootWorkItem.Items[Menu] as DevExpress.XtraBars.BarSubItem;
            AgregaMenu(subMenu, barItem);
        }

        private static DevExpress.XtraBars.BarButtonItem ObtenerBarItem(string Texto, System.Drawing.Image Imagen, System.Windows.Forms.Shortcut ShorCut)
        {
            DevExpress.XtraBars.BarButtonItem barItem = new DevExpress.XtraBars.BarButtonItem();

            barItem.Caption = Texto;
            barItem.LargeGlyph = Imagen;
            barItem.Id = 99;
            barItem.Name = "itm" + Texto;
            barItem.ShortCut = ShorCut;
            barItem.Hint = ShorCut == System.Windows.Forms.Shortcut.None ? string.Empty : ShorCut.ToString();
            return barItem;
        }

        private static DevExpress.XtraBars.BarButtonItem ObtenerBarItemSmall(string Texto, System.Drawing.Image Imagen, System.Windows.Forms.Shortcut ShorCut)
        {
            DevExpress.XtraBars.BarButtonItem barItem = new DevExpress.XtraBars.BarButtonItem();

            barItem.Caption = Texto;
            barItem.Glyph = Imagen;
            barItem.Id = 99;
            barItem.Name = "itm" + Texto;
            barItem.ShortCut = ShorCut;
            barItem.Hint = ShorCut.ToString();
            return barItem;
        }

        private void AgregaMenu(DevExpress.XtraBars.BarSubItem subMenu, DevExpress.XtraBars.BarButtonItem barItem)
        {
            subMenu.LinksPersistInfo.Add(new DevExpress.XtraBars.LinkPersistInfo(barItem));
        }

        public void LimpiaMenuCatalogo()
        {
            try
            {
                DevExpress.XtraBars.Ribbon.RibbonControl ribbon = _WorkItem.RootWorkItem.Items[UIExtensionSiteNames.Ribbon] as DevExpress.XtraBars.Ribbon.RibbonControl;
                DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage = ribbon.Pages["Menú"];

                foreach (DevExpress.XtraBars.Ribbon.RibbonPageGroup grupo in ribbonPage.Groups)
                {
                    while (grupo.ItemLinks.Count > 0)
                    {
                        grupo.ItemLinks[0].Item.ShortCut = System.Windows.Forms.Shortcut.None;

                        grupo.ItemLinks[0].Dispose();
                    }
                }

                while (ribbonPage.Groups.Count > 0)
                {
                    ribbonPage.Groups[0].Dispose();
                }
            }
            catch { }
        }

        public void AgregaBotonMenuCatalogoGrupo(string Grupo, string Texto, DevExpress.XtraBars.ItemClickEventHandler Evento, string imagen, System.Windows.Forms.Shortcut ShorCut)
        {
            string RutaImagenCo = (string)System.Configuration.ConfigurationSettings.AppSettings["RutaImagenCo"];

            if (RutaImagenCo == null || RutaImagenCo == "")
                RutaImagenCo = "C:\\";

            System.Drawing.Image icono = ImagenSoft.Librerias.Utilerias.DameIcono(imagen, RutaImagenCo);
            if (icono == null)
            {
                icono = (System.Drawing.Image)EstandarCliente.Infrastructure.Module.Properties.Resources.ResourceManager.GetObject(imagen);
            }

            DevExpress.XtraBars.Ribbon.RibbonControl ribbon = _WorkItem.RootWorkItem.Items[UIExtensionSiteNames.Ribbon] as DevExpress.XtraBars.Ribbon.RibbonControl;
            DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage = ribbon.Pages["Menú"];

            DevExpress.XtraBars.Ribbon.RibbonPageGroup grupo = ribbonPage.Groups[Grupo];

            if (grupo == null)
            {
                grupo = new DevExpress.XtraBars.Ribbon.RibbonPageGroup(Grupo);
                grupo.Name = Grupo;
                grupo.AllowTextClipping = false;
                ribbonPage.Groups.Add(grupo);
            }

            DevExpress.XtraBars.BarButtonItem barItem = ObtenerBarItem(Texto, icono, ShorCut);
            barItem.ItemClick += Evento;

            ribbon.Items.Add(barItem);

            grupo.ItemLinks.Add(barItem);
        }

        public void AgregarBotonSeccionMenu(string Seccion, string Grupo, string Texto, DevExpress.XtraBars.ItemClickEventHandler Evento, System.Drawing.Image ImagenGrande, System.Drawing.Image ImagenChica, System.Windows.Forms.Shortcut ShorCut)
        {
            string OpcionesMenu = System.Configuration.ConfigurationSettings.AppSettings["OpcionesMenu"];

            int CantOpcionesMenu = 3;
            if (OpcionesMenu != null) { int.TryParse(OpcionesMenu, out CantOpcionesMenu); }

            DevExpress.XtraBars.Ribbon.RibbonControl ribbon = _WorkItem.RootWorkItem.Items[UIExtensionSiteNames.Ribbon] as DevExpress.XtraBars.Ribbon.RibbonControl;
            DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage = ribbon.Pages[Seccion];

            if (ribbonPage == null)
            {
                ribbonPage = new DevExpress.XtraBars.Ribbon.RibbonPage(Seccion);
                ribbon.Pages.Insert(ribbon.Pages.Count - 1, ribbonPage);
            }

            DevExpress.XtraBars.Ribbon.RibbonPageGroup grupo = ribbonPage.Groups[Grupo];

            if (grupo == null)
            {
                grupo = new DevExpress.XtraBars.Ribbon.RibbonPageGroup(Grupo);
                grupo.Name = Grupo;
                ribbonPage.Groups.Add(grupo);
            }//***********************************
            if (grupo.ItemLinks.Count >= CantOpcionesMenu)
            {
                DevExpress.XtraBars.BarItem sitem = null;// ribbon.Items["Más"];

                foreach (DevExpress.XtraBars.BarItem itm in ribbon.Items)
                {
                    if (itm.Name == Grupo + "Más")
                    {
                        sitem = itm;
                        break;
                    }
                }

                DevExpress.XtraBars.BarButtonItem barSubItem = ObtenerBarItem(Texto, ImagenGrande, ShorCut);
                barSubItem.Glyph = ImagenChica;
                barSubItem.ItemClick += Evento;

                if (sitem == null)
                {
                    DevExpress.XtraBars.BarSubItem subItem = ribbon.Items.CreateMenu("Más", barSubItem);
                    subItem.Name = Grupo + "Más";
                    subItem.LargeGlyph = ImagenSoft.Librerias.Utilerias.DameIcono("Mas", "c:\\");

                    ribbon.Items.Add(barSubItem);
                    grupo.ItemLinks.Add(subItem);
                    sitem = subItem;
                }
                else
                {

                    DevExpress.XtraBars.BarButtonItem barbuttonItem = ObtenerBarItem(Texto, ImagenGrande, ShorCut);
                    barbuttonItem.ItemClick += Evento;
                    barbuttonItem.Glyph = ImagenChica;

                    //ribbon.Items.Add(barItem);

                    ////grupo.ItemLinks.Add(barItem);
                    ribbon.Items.Add(barbuttonItem);
                    (sitem as DevExpress.XtraBars.BarSubItem).ItemLinks.Add(barbuttonItem);
                }

                //sitem.ItemLinks.Add(barItem);

                //return;
            }
            else
            {
                //*****************************
                DevExpress.XtraBars.BarButtonItem barItem = ObtenerBarItem(Texto, ImagenGrande, ShorCut);
                barItem.ItemClick += Evento;
                barItem.Glyph = ImagenChica;

                ribbon.Items.Add(barItem);

                grupo.ItemLinks.Add(barItem);
            }
        }

        public void AgregarBotonSeccionMenu(string Seccion, string Grupo, string Texto, DevExpress.XtraBars.ItemClickEventHandler Evento, System.Drawing.Image Imagen, System.Windows.Forms.Shortcut ShorCut)
        {
            AgregarBotonSeccionMenu(Seccion, Grupo, Texto, Evento, Imagen, Imagen, ShorCut);
            //string OpcionesMenu = System.Configuration.ConfigurationSettings.AppSettings["OpcionesMenu"];

            //int CantOpcionesMenu = 3;
            //try
            //{
            //    if (OpcionesMenu != null) { CantOpcionesMenu = Convert.ToInt32(OpcionesMenu); }
            //}
            //catch
            //{
            //}

            //DevExpress.XtraBars.Ribbon.RibbonControl ribbon = _WorkItem.RootWorkItem.Items[UIExtensionSiteNames.Ribbon] as DevExpress.XtraBars.Ribbon.RibbonControl;
            //DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage =  ribbon.Pages[Seccion];

            //if (ribbonPage == null)
            //{
            //    ribbonPage = new DevExpress.XtraBars.Ribbon.RibbonPage(Seccion);
            //    ribbon.Pages.Add(ribbonPage);
            //}

            //DevExpress.XtraBars.Ribbon.RibbonPageGroup grupo = ribbonPage.Groups[Grupo];

            //if (grupo == null)
            //{
            //    grupo = new DevExpress.XtraBars.Ribbon.RibbonPageGroup(Grupo);
            //    grupo.Name = Grupo;
            //    ribbonPage.Groups.Add(grupo);
            //}//***********************************
            //if (grupo.ItemLinks.Count >= CantOpcionesMenu)
            //{
            //    DevExpress.XtraBars.BarItem sitem = null;// ribbon.Items["Más"];

            //    foreach (DevExpress.XtraBars.BarItem itm in ribbon.Items)
            //    {
            //        if (itm.Name == Grupo+"Más")
            //        {
            //            sitem = itm;
            //            break;
            //        }
            //    }

            //    DevExpress.XtraBars.BarButtonItem barSubItem = ObtenerBarItem(Texto, Imagen, ShorCut);
            //    barSubItem.ItemClick += Evento;

            //    if (sitem == null)
            //    {
            //        DevExpress.XtraBars.BarSubItem subItem = ribbon.Items.CreateMenu("Más", barSubItem);
            //        subItem.Name = Grupo+"Más";
            //        subItem.LargeGlyph = ImagenSoft.Librerias.Utilerias.DameIcono("Mas", "c:\\");

            //        ribbon.Items.Add(barSubItem);
            //        grupo.ItemLinks.Add(subItem);
            //        sitem = subItem;
            //    }
            //    else
            //    {

            //        DevExpress.XtraBars.BarButtonItem barbuttonItem = ObtenerBarItem(Texto, Imagen, ShorCut);
            //        barbuttonItem.ItemClick += Evento;

            //        //ribbon.Items.Add(barItem);

            //        ////grupo.ItemLinks.Add(barItem);
            //        ribbon.Items.Add(barbuttonItem);
            //        (sitem as DevExpress.XtraBars.BarSubItem).ItemLinks.Add(barbuttonItem);
            //    }

            //    //sitem.ItemLinks.Add(barItem);

            //    //return;
            //}
            //else
            //{
            //    //*****************************
            //    DevExpress.XtraBars.BarButtonItem barItem = ObtenerBarItem(Texto, Imagen, ShorCut);
            //    barItem.ItemClick += Evento;

            //    ribbon.Items.Add(barItem);

            //    grupo.ItemLinks.Add(barItem);
            //}
        }

        public void AgregarBotonSmallSeccionMenu(string Seccion, string Grupo, string Texto, DevExpress.XtraBars.ItemClickEventHandler Evento, System.Drawing.Image Imagen, System.Windows.Forms.Shortcut ShorCut)
        {
            DevExpress.XtraBars.Ribbon.RibbonControl ribbon = _WorkItem.RootWorkItem.Items[UIExtensionSiteNames.Ribbon] as DevExpress.XtraBars.Ribbon.RibbonControl;
            DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage = ribbon.Pages[Seccion];

            if (ribbonPage == null)
            {
                ribbonPage = new DevExpress.XtraBars.Ribbon.RibbonPage(Seccion);
                ribbon.Pages.Add(ribbonPage);
            }

            DevExpress.XtraBars.Ribbon.RibbonPageGroup grupo = ribbonPage.Groups[Grupo];

            if (grupo == null)
            {
                grupo = new DevExpress.XtraBars.Ribbon.RibbonPageGroup(Grupo);
                grupo.Name = Grupo;
                ribbonPage.Groups.Add(grupo);
            }

            DevExpress.XtraBars.BarButtonItem barItem = ObtenerBarItemSmall(Texto, Imagen, ShorCut);
            barItem.ItemClick += Evento;

            ribbon.Items.Add(barItem);

            grupo.ItemLinks.Add(barItem);
        }

        public void HabilitarBotonSeccionMenu(string Seccion, string Grupo, string Texto, bool habilita)
        {
            try
            {
                DevExpress.XtraBars.Ribbon.RibbonControl ribbon = _WorkItem.RootWorkItem.Items[UIExtensionSiteNames.Ribbon] as DevExpress.XtraBars.Ribbon.RibbonControl;
                DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage = ribbon.Pages[Seccion];

                if (ribbonPage == null)
                {
                    return;
                    //throw new Exception("No se encontro Sección " + Seccion + "en el menpu principal");
                }

                DevExpress.XtraBars.Ribbon.RibbonPageGroup grupo = ribbonPage.Groups[Grupo];

                if (grupo == null)
                {
                    return;
                    //throw new Exception("No se encontro grupo " + Grupo + "en la sección " + Seccion);
                }

                foreach (DevExpress.XtraBars.BarItemLink _item in grupo.ItemLinks)
                {
                    if (_item.Item.Caption.Equals(Texto))
                    {
                        _item.Item.Enabled = habilita;
                    }
                }
            }
            catch { }
        }

        public void OcultarBotonSeccionMenu(string Seccion, string Grupo, string Texto, bool Ocultar)
        {
            DevExpress.XtraBars.Ribbon.RibbonControl ribbon = _WorkItem.RootWorkItem.Items[UIExtensionSiteNames.Ribbon] as DevExpress.XtraBars.Ribbon.RibbonControl;
            DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage = ribbon.Pages[Seccion];

            if (ribbonPage == null)
            {
                return;
                throw new Exception("No se encontro Sección " + Seccion + "en el menpu principal");
            }

            DevExpress.XtraBars.Ribbon.RibbonPageGroup grupo = ribbonPage.Groups[Grupo];

            if (grupo == null)
            {
                return;
                throw new Exception("No se encontro grupo " + Grupo + "en la sección " + Seccion);
            }

            foreach (DevExpress.XtraBars.BarItemLink _item in grupo.ItemLinks)
            {
                if (_item.Item.Caption.Equals(Texto))
                {
                    if (Ocultar)
                    {
                        _item.Item.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    }
                    else
                    {
                        _item.Item.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    }
                }
            }
        }

        public void OcultaGrupoSeccionMenu(string Seccion, string Grupo)
        {
            DevExpress.XtraBars.Ribbon.RibbonControl ribbon = _WorkItem.RootWorkItem.Items[UIExtensionSiteNames.Ribbon] as DevExpress.XtraBars.Ribbon.RibbonControl;
            DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage = ribbon.Pages[Seccion];

            if (ribbonPage == null)
            {
                return;
                throw new Exception("No se encontro Sección " + Seccion + "en el menpu principal");
            }

            DevExpress.XtraBars.Ribbon.RibbonPageGroup grupo = ribbonPage.Groups[Grupo];

            if (grupo == null)
            {
                return;
                throw new Exception("No se encontro grupo " + Grupo + "en la sección " + Seccion);
            }
            else
            {
                grupo.Visible = false;
            }
        }

        public void OcultaSeccionMenu(string Seccion)
        {
            DevExpress.XtraBars.Ribbon.RibbonControl ribbon = _WorkItem.RootWorkItem.Items[UIExtensionSiteNames.Ribbon] as DevExpress.XtraBars.Ribbon.RibbonControl;
            DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage = ribbon.Pages[Seccion];

            if (ribbonPage == null)
            {
                return;
                throw new Exception("No se encontro Sección " + Seccion + "en el menpu principal");
            }
            else
            {
                ribbonPage.Visible = false;
            }
        }

        public void MostrarSeccionMenu(string Seccion)
        {
            DevExpress.XtraBars.Ribbon.RibbonControl ribbon = _WorkItem.RootWorkItem.Items[UIExtensionSiteNames.Ribbon] as DevExpress.XtraBars.Ribbon.RibbonControl;
            DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage = ribbon.Pages[Seccion];

            if (ribbonPage == null)
            {
                return;
                throw new Exception("No se encontro Sección " + Seccion + "en el menpu principal");
            }
            else
            {
                ribbonPage.Visible = true;
            }
        }

        public List<string> ObtenerNombresRibbone()
        {
            DevExpress.XtraBars.Ribbon.RibbonControl ribbon = _WorkItem.RootWorkItem.Items[UIExtensionSiteNames.Ribbon] as DevExpress.XtraBars.Ribbon.RibbonControl;
            List<string> result = new List<string>(ribbon.Pages.Count);
            foreach (DevExpress.XtraBars.Ribbon.RibbonPage item in ribbon.Pages)
            {
                result.Add(item.Text);
            }
            return result;
        }

    }
}
