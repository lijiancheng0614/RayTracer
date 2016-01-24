using System.Collections.Generic;

namespace RayTracer.Model.ObjModels
{
    class Face
    {
        string materialName;

        public string MaterialName
        {
            get { return materialName; }
        }

        List<FaceItem> faceItems;

        public List<FaceItem> FaceItems
        {
            get { return faceItems; }
        }

        public Face(string materialName)
        {
            this.materialName = materialName;
            faceItems = new List<FaceItem>();
        }
    }
}
