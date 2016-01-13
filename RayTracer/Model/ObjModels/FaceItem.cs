
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

        public FaceItem(int _vertexIndex, int _textureIndex, int _normalIndex)
        {
            vertexIndex = _vertexIndex;
            textureIndex = _textureIndex;
            normalIndex = _normalIndex;
        }
    }
}
