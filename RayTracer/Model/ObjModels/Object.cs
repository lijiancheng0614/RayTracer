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

        public Object(string _name)
        {
            name = _name;
            faces = new List<Face>();
        }
    }
}
