using System.Collections.Generic;

namespace RayTracer.Model.ObjModels
{
    class Object
    {
        string name;

        public string Name
        {
            get { return name; }
        }

        List<Face> faces;

        public List<Face> Faces
        {
            get { return faces; }
        }

        public Object(string name)
        {
            this.name = name;
            faces = new List<Face>();
        }
    }
}
