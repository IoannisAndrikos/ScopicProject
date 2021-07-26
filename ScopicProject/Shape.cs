using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kitware.VTK;

namespace ScopicProject
{
    class Shape
    {
        public Shape()
        {

        }

        public geometryVTK createCube()
        {
            Cube cube = new Cube();
            vtkPolyData item = cube.createCubeItem();
            vtkPolyDataMapper mapper = vtkPolyDataMapper.New();
            mapper.SetInput(item);
            vtkActor actor = vtkActor.New();
            actor.SetMapper(mapper);
            actor.GetProperty().SetColor(1, 1, 1);
            actor.GetProperty().SetOpacity(1);
            return new geometryVTK() { type = geometryVTK.shapeType.cube, actor = actor };
        }

        public void createShare()
        {
            
        }

        public void createPyramid()
        {

        }
    }

    public class geometryVTK
    {
        public enum shapeType { cube, shpere, pyramid, None }

        public shapeType type { set; get; }
        public vtkActor actor { set; get; }
    }
}
