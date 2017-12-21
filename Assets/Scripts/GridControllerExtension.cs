using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public static class GridControllerExtension
    {
        /// <summary>
        /// data una posizione restituisce la cella corrispondente
        /// </summary>
        /// <param name="_position">la posizione da controllare</param>
        /// <returns>la cella che si trova in quella posizione</returns>
        public static Cell GetCellFromPosition(this GridController _gridCtrl, Vector3 _position)
        {
            Cell resultant;
            int[] indexes = _gridCtrl.GetCoordinatesByPosition(_position);
            Cell[,,] matrix = _gridCtrl.GetCellsMatrix();

            if (matrix.GetLength(0) > indexes[0] && matrix.GetLength(1) > indexes[1] && matrix.GetLength(2) > indexes[2])
                resultant = matrix[indexes[0], indexes[1], indexes[2]];
            else
                resultant = null;

            return resultant;
        }
        /// <summary>
        /// Return a specific cell based on coordinates
        /// </summary>
        /// <param name="_gridCtrl"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Cell GetCellByCoordinates(this GridController _gridCtrl, int i, int j, int k)
        {
            Cell resultant;
            resultant = _gridCtrl.GetCellsMatrix()[i, j, k];
            return resultant;
        }
        /// <summary>
        /// Return the center postion of the cell
        /// </summary>
        /// <param name="_gridCtrl"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Vector3 GetPositionByCoordinates(this GridController _gridCtrl, int i, int j, int k)
        {

            Vector3 spacePos = new Vector3(i * _gridCtrl.SectorData.Diameter.x, j * _gridCtrl.SectorData.Diameter.y, k * _gridCtrl.SectorData.Diameter.z);
            spacePos += _gridCtrl.SectorData.Radius;
            //spacePos -= _gridCtrl.GetOffset();

            return spacePos;
        }
        //Associate the coordinate of clostest Cell center (or cell array index)
        //to the position vector
        //By Math: this is actually the metric conversion of the Grid Sub Vectorial Space, but
        //the int cast on the normalized position
        public static int[] GetCoordinatesByPosition(this GridController _gridCtrl, Vector3 _position)
        {
            Vector3 spacePos = _position - _gridCtrl.transform.position /*+ _gridCtrl.GetOffset()*/;
            float[] centerDis = new float[3];
            for (int i = 0; i < 3; i++)
            {
                centerDis[i] = _gridCtrl.SectorData.Diameter[i] + _gridCtrl.ResolutionCorrection[i];
            }

            int[] coordinates = new int[]
            {
                centerDis[0] != 0 ?(int)(spacePos.y/centerDis[0]): 0,
                centerDis[1] != 0 ?(int)(spacePos.x/centerDis[1]): 0,
                centerDis[2] != 0 ?(int)(spacePos.x/centerDis[2]): 0,
            };

            return coordinates;
        }
    }
}
