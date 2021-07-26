using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kitware.VTK;

namespace ScopicProject
{
    class Cube
    {
        private double cubeEdgeSize = 1;

        int[][] faceOrdering = new [] {
            new[] { 0, 1, 2, 3 },
            new[] { 4, 5, 6, 7 }, 
            new[] { 0, 1, 5, 4 },
            new[] { 1, 2, 6, 5 }, 
            new[]{ 2, 3, 7, 6 },
            new[]{ 3, 0, 4, 7 } };

        public Cube() { }

        public vtkPolyData createCubeItem()
        {
            vtkPolyData polyData = new vtkPolyData();
            polyData.SetPoints(createVertices());
            polyData.SetPolys(createFaces());

            return polyData;
        }


        private vtkPoints createVertices()
        {
            double[] center = getRandomCenter();
            vtkPoints points = new vtkPoints();

            //define eight corners
            points.InsertNextPoint(center[0] - cubeEdgeSize, center[0] - cubeEdgeSize, center[0] - cubeEdgeSize);
            points.InsertNextPoint(center[0] - cubeEdgeSize, center[0] + cubeEdgeSize, center[0] - cubeEdgeSize);
            points.InsertNextPoint(center[0] + cubeEdgeSize, center[0] + cubeEdgeSize, center[0] - cubeEdgeSize);
            points.InsertNextPoint(center[0] + cubeEdgeSize, center[0] - cubeEdgeSize, center[0] - cubeEdgeSize);
            points.InsertNextPoint(center[0] - cubeEdgeSize, center[0] - cubeEdgeSize, center[0] + cubeEdgeSize);
            points.InsertNextPoint(center[0] - cubeEdgeSize, center[0] + cubeEdgeSize, center[0] + cubeEdgeSize);
            points.InsertNextPoint(center[0] + cubeEdgeSize, center[0] + cubeEdgeSize, center[0] + cubeEdgeSize);
            points.InsertNextPoint(center[0] + cubeEdgeSize, center[0] - cubeEdgeSize, center[0] + cubeEdgeSize);

            return points;
        }

        private vtkCellArray createFaces()
        {
            //connect points with triangles according to face ordering
            vtkCellArray triangles = new vtkCellArray();
            foreach(int[] i in faceOrdering)
            {
                triangles.InsertNextCell(mkVtkIdList(i));
            }

            return triangles;
        }


        private double[] getRandomCenter()
        {
            double max = 10 - cubeEdgeSize;
            double min = -10 + cubeEdgeSize;

            double[] center = new double[3];
            Random random = new Random();
            center[0] =  random.NextDouble() * (max - min) + min;
            center[1] =  random.NextDouble() * (max - min) + min;
            center[2] =  random.NextDouble() * (max - min) + min;
            return center;  
        }

        private vtkIdList mkVtkIdList(int[] it)
        {
            vtkIdList list = new vtkIdList();
            foreach(int i in it)
            {
                list.InsertNextId(i);
            }
            return list;
        }
    }
}
