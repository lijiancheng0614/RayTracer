
namespace RayTracer.Model.ObjModels
{
    class FaceItem
    {
        int vertexIndex = -1;

        public int VertexIndex
        {
            get { return vertexIndex; }
        }

        int textureIndex = -1;

        public int TextureIndex
        {
            get { return textureIndex; }
        }

        int normalIndex = -1;

        public int NormalIndex
        {
            get { return normalIndex; }
        }

        public FaceItem(int vertexIndex, int textureIndex, int normalIndex)
        {
            this.vertexIndex = vertexIndex;
            this.textureIndex = textureIndex;
            this.normalIndex = normalIndex;
        }
    }
}
