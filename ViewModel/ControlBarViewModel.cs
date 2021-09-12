using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Tour_management.ViewModel
{
    public class ControlBarViewModel : BaseViewModel
    {
        #region Properties
        public ICommand CloseWindowCommmand { get; set; }
        public ICommand MinimizeWindowCommmand { get; set; }
        public ICommand MaximizeWindowCommmand { get; set; }
        public ICommand MouseMoveWindowCommand { get; set; }
        #endregion

        public ControlBarViewModel()
        {
            MouseMoveWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; },
                (p) => {
                    Window w = Window.GetWindow(p);
                    if (w != null)
                    {
                        w.DragMove();
                    }
                });

            MaximizeWindowCommmand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; },
                (p) => {
                    Window w = Window.GetWindow(p);
                    if (w.WindowState != WindowState.Maximized)
                    {
                        w.WindowState = WindowState.Maximized;
                    }
                    else
                    {
                        w.WindowState = WindowState.Normal;
                    }
                });

            MinimizeWindowCommmand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; },
                (p) => {
                    Window w = Window.GetWindow(p);
                    if (w.WindowState != WindowState.Minimized)
                    {
                        w.WindowState = WindowState.Minimized;
                    }
                });

            CloseWindowCommmand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; },
                (p) => {
                    Window w = Window.GetWindow(p);
                    if (w != null)
                    {
                        w.Close();
                    }
                });
        }
    }
}
