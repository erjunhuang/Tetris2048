/*
uGui-Effect-Tool
Copyright (c) 2015 WestHillApps (Hironari Nishioka)
This software is released under the MIT License.
http://opensource.org/licenses/mit-license.php
*/
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace QGame.Utils
{
    [AddComponentMenu ("UI/Effects/Gradient Color")]
    [RequireComponent (typeof (Graphic))]
    public class GradientColor : BaseMeshEffect
    {
        public enum DIRECTION
        {
            Vertical,
            Horizontal,
            Both,
        }

        public DIRECTION direction = DIRECTION.Both;
        public Color colorTop = Color.white;
        public Color colorBottom = Color.black;
        public Color colorLeft = Color.red;
        public Color colorRight = Color.blue;


        public void ModifyVertices (List<UIVertex> vList)
        {
            if (IsActive () == false || vList == null || vList.Count == 0) {
                return;
            }

            float topX = 0f, topY = 0f, bottomX = 0f, bottomY = 0f;
            foreach (var vertex in vList) {
                topX = Mathf.Max (topX, vertex.position.x);
                topY = Mathf.Max (topY, vertex.position.y);
                bottomX = Mathf.Min (bottomX, vertex.position.x);
                bottomY = Mathf.Min (bottomY, vertex.position.y);
            }
            float width = topX - bottomX;
            float height = topY - bottomY;

            UIVertex tempVertex = vList[0];
            for (int i = 0; i < vList.Count; i++) {
                tempVertex = vList[i];
                byte orgAlpha = tempVertex.color.a;
                Color colorOrg = tempVertex.color;
                Color colorV = Color.Lerp (colorBottom, colorTop, (tempVertex.position.y - bottomY) / height);
                Color colorH = Color.Lerp (colorLeft, colorRight, (tempVertex.position.x - bottomX) / width);
                switch (direction) {
                    case DIRECTION.Both:
                        tempVertex.color = colorOrg * colorV * colorH;
                        break;
                    case DIRECTION.Vertical:
                        tempVertex.color = colorOrg * colorV;
                        break;
                    case DIRECTION.Horizontal:
                        tempVertex.color = colorOrg * colorH;
                        break;
                }
                tempVertex.color.a = orgAlpha;
                vList[i] = tempVertex;
            }
        }

        /// <summary>
        /// Refresh Gradient Color on playing.
        /// </summary>
        public void Refresh ()
        {
            if (graphic != null) {
                graphic.SetVerticesDirty ();
            }
        }
        #if UNITY_5_2_2
        public override void ModifyMesh(Mesh mesh)
        {
            if (!this.IsActive())
                return;

            List<UIVertex> list = new List<UIVertex>();
            using (VertexHelper vertexHelper = new VertexHelper(mesh))
            {
                vertexHelper.GetUIVertexStream(list);
            }

            ModifyVertices(list);  // calls the old ModifyVertices which was used on pre 5.2

            using (VertexHelper vertexHelper2 = new VertexHelper())
            {
                vertexHelper2.AddUIVertexTriangleStream(list);
                vertexHelper2.FillMesh(mesh);
            }
        }
         #else 
        public override void ModifyMesh(VertexHelper vh)
        {

            if (!this.IsActive())
                return;

            List<UIVertex> list = new List<UIVertex>();
            vh.GetUIVertexStream(list);
            ModifyVertices(list);
            vh.AddUIVertexTriangleStream(list);
        }
        #endif
    }
}
