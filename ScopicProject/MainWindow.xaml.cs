using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Kitware.VTK;

namespace ScopicProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Forms.Integration.WindowsFormsHost host;
        public RenderWindowControl myRenderWindowControl;
        public vtkRenderer renderer;
        vtkAxesActor axes;
        vtkRenderWindowInteractor interactor;
        vtkInteractorStyleTrackballCamera trackballCamera;
        Shape shape;
        int cubeCount = 0;
        List<geometryVTK> geometryItems = new List<geometryVTK>();

        public MainWindow()
        {
            InitializeComponent();
            this.shape = new Shape();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            host = new System.Windows.Forms.Integration.WindowsFormsHost();
            myRenderWindowControl = new RenderWindowControl();
            host.Child = myRenderWindowControl;
            myRenderWindowControl.SetBounds(0, 0, 0, 0); // not too big in case it disappears.                          
            host.Child = myRenderWindowControl;
            host.Margin = new Thickness(0, 0, 0, 0);
            host.HorizontalAlignment = HorizontalAlignment.Stretch;
            host.VerticalAlignment = VerticalAlignment.Stretch;
            renderGrid.Children.Add(host);

            renderer = myRenderWindowControl.RenderWindow.GetRenderers().GetFirstRenderer();

            //add axes
            axes = new vtkAxesActor();
            vtkTransform transform = new vtkTransform();
            transform.Translate(0.0, 0.0, 0.0);
            axes.SetTotalLength(10, 10, 10);
            renderer.AddActor(axes);

            //add picker
            vtkPointPicker picker = new vtkPointPicker();
            renderer.GetRenderWindow().LeftButtonPressEvt += Renderer_LeftButtonPressEvt;

            //interactor
            interactor = new vtkRenderWindowInteractor();
            interactor.SetPicker(picker);
            interactor.SetRenderWindow(renderer.GetRenderWindow());
            trackballCamera = new vtkInteractorStyleTrackballCamera();

            interactor.SetInteractorStyle(trackballCamera);
            trackballCamera.SetCurrentRenderer(renderer);
            trackballCamera.SetInteractor(interactor);

            trackballCamera.LeftButtonPressEvt += Renderer_LeftButtonPressEvt;

            interactor.Start();
        }

        private void Renderer_LeftButtonPressEvt(vtkObject sender, vtkObjectEventArgs e)
        {
            this.interactor.GetPicker().Pick(this.interactor.GetEventPosition()[0],
                                                this.interactor.GetEventPosition()[1],
                                                0,
                                                this.interactor.GetRenderWindow().GetRenderers().GetFirstRenderer());
            geometryVTK.shapeType selectedType = checkIfClickBelongToShape(this.interactor.GetPicker().GetPickPosition());

            selectedObject_label.Content = selectedType + " object is selected";
        }


        private void cubeButton_Click(object sender, RoutedEventArgs e)
        {
            if (cubeCount >= 1) return;

            geometryVTK geometry = shape.createCube();
            geometryItems.Add(geometry);
            renderer.AddActor(geometry.actor);
            renderer.ResetCamera();
            myRenderWindowControl.RenderWindow.Render();
            cubeCount = cubeCount + 1;
        }


        private void zoomButton_Click(object sender, RoutedEventArgs e)
        {
            renderer.GetActiveCamera().Zoom(1.2);
            myRenderWindowControl.RenderWindow.Render();
        }


        private void PanButton_Click(object sender, RoutedEventArgs e)
        {
            trackballCamera.Pan();
        }


        private void RotateButton_Click(object sender, RoutedEventArgs e)
        {
            trackballCamera.Rotate();
        }

            
        //check selected object
        private geometryVTK.shapeType checkIfClickBelongToShape(double[] clickPoint)
        {
            double[] bounds;
            foreach(geometryVTK geometry in geometryItems)
            {
                bounds = geometry.actor.GetBounds();
                if((clickPoint[0] >= bounds[0] && clickPoint[0] <= bounds[1]) 
                    && (clickPoint[1] >= bounds[2] && clickPoint[0] <= bounds[3])
                    && (clickPoint[2] >= bounds[4] && clickPoint[0] <= bounds[5]))
                {
                     return geometry.type;
                }
            }
            return geometryVTK.shapeType.None;
        }

       
    }

   
}
