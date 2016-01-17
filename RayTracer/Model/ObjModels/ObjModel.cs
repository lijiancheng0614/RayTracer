using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using RayTracer.Model.Geometries;

namespace RayTracer.Model.ObjModels
{
    /// <summary>
    /// Wavefront .obj file. Reference: https://en.wikipedia.org/wiki/Wavefront_.obj_file
    /// </summary>
    class ObjModel
    {
        List<Triangle> triangles = new List<Triangle>();

        public List<Triangle> Triangles
        {
            get { return triangles; }
        }

        List<Vector3> vertices = new List<Vector3>();
        List<Texture> textures = new List<Texture>();
        List<Vector3> normals = new List<Vector3>();
        Dictionary<string, Object> objects = new Dictionary<string, Object>();
        Dictionary<string, Material> materials = new Dictionary<string, Material>();
        Materials.PhongMaterial defaultMaterial = new Materials.PhongMaterial(Model.Color.White, Model.Color.Black, 0);

        public ObjModel(string objModelPath)
        {
            Load(objModelPath);
            GetTriangles();
        }

        /// <summary>
        /// Load an obj file.
        /// </summary>
        /// <param name="objModelPath">obj file path.</param>
        private void Load(string objModelPath)
        {
            if (!File.Exists(objModelPath))
            {
                throw new ArgumentException("File not found!", objModelPath);
            }

            string objModelFolder = Path.GetDirectoryName(objModelPath) ?? string.Empty;
            string line = null;
            string lastObjectName = null;
            string lastObjectMaterial = null;
            var streamReader = new StreamReader(objModelPath);

            while ((line = streamReader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line) || line[0] == '#')
                {
                    continue;
                }
                line = line.Trim();
                string[] s = Regex.Split(line, "\\s+");
                if (s.Length < 2)
                {
                    Console.WriteLine("No arguments for this line: " + line + ".");
                    continue;
                }
                if (s[0] == "v" && s.Length >= 4)
                {
                    #region vertex
                    double x, y, z;
                    if (double.TryParse(s[1], out x) && double.TryParse(s[2], out y) && double.TryParse(s[3], out z))
                    {
                        vertices.Add(new Vector3(x, y, z));
                    }
                    else
                    {
                        Console.WriteLine("Cannot parse vertex line: " + line + ".");
                    }
                    #endregion
                }
                else if (s[0] == "vn" && s.Length >= 4)
                {
                    #region normal
                    double x, y, z;
                    if (double.TryParse(s[1], out x) && double.TryParse(s[2], out y) && double.TryParse(s[3], out z))
                    {
                        normals.Add(new Vector3(x, y, z));
                    }
                    else
                    {
                        Console.WriteLine("Cannot parse normal line: " + line + ".");
                    }
                    #endregion
                }
                else if (s[0] == "vt" && s.Length >= 3)
                {
                    #region texture
                    double x, y;
                    if (double.TryParse(s[1], out x) && double.TryParse(s[2], out y))
                    {
                        textures.Add(new Texture(x, y));
                    }
                    else
                    {
                        Console.WriteLine("Cannot parse texture line: " + line + ".");
                    }
                    #endregion
                }
                else if (s[0] == "vp" && s.Length >= 2)
                {
                    // TODO: parameter space vertex
                }
                else if (s[0] == "f")
                {
                    #region face
                    if (objects.Count == 0)
                    {
                        objects["Untitled"] = new Object("Untitled");
                        lastObjectName = "Untitled";
                    }
                    Face face = new Face(lastObjectMaterial);
                    foreach (var i in s.Skip(1))
                    {
                        string[] tokens = i.Split(new char[] { '/' }, StringSplitOptions.None);
                        if (tokens.Length == 0)
                        {
                            Console.WriteLine("Cannot found vertex in face line: " + line + ".");
                            continue;
                        }
                        int vertexIndex = 0, textureIndex = 0, normalIndex = 0;
                        if (string.IsNullOrWhiteSpace(tokens[0]) || !int.TryParse(tokens[0], out vertexIndex))
                        {
                            Console.WriteLine("Cannot parse face line: " + line + ".");
                            continue;
                        }
                        if (tokens.Length >= 2 && !string.IsNullOrWhiteSpace(tokens[1]))
                        {
                            if (!int.TryParse(tokens[1], out textureIndex))
                            {
                                Console.WriteLine("Cannot parse textureIndex in face line: " + line + ".");
                            }
                        }
                        if (tokens.Length >= 3 && !string.IsNullOrWhiteSpace(tokens[2]))
                        {
                            if (!int.TryParse(tokens[2], out normalIndex))
                            {
                                Console.WriteLine("Cannot parse normalIndex in face line: " + line + ".");
                            }
                        }
                        face.FaceItems.Add(new FaceItem(vertexIndex - 1, textureIndex - 1, normalIndex - 1));
                    }
                    objects[lastObjectName].Faces.Add(face);
                    #endregion
                }
                else if (s[0] == "o" && s.Length >= 2)
                {
                    #region object
                    string objectName = string.Join(" ", s.Skip(1));
                    objects[objectName] = new Object(objectName);
                    lastObjectName = objectName;
                    #endregion
                }
                else if (s[0] == "mtllib" && s.Length >= 2)
                {
                    #region load material
                    string materialFileName = string.Join(" ", s.Skip(1));
                    LoadMaterial(Path.Combine(objModelFolder, materialFileName));
                    #endregion
                }
                else if (s[0] == "usemtl" && s.Length >= 2)
                {
                    #region use material
                    string materialName = string.Join(" ", s.Skip(1));
                    if (materials.ContainsKey(materialName))
                    {
                        lastObjectMaterial = materialName;
                    }
                    else
                    {
                        Console.WriteLine("Cannot find the material: " + materialName + ".");
                    }
                    #endregion
                }
                else if (s[0] == "g")
                {
                    // TODO: group
                }
                else if (s[0] == "s")
                {
                    // TODO: smoothness
                }
                else
                {
                    Console.WriteLine("Unknown line: " + line + "\n");
                }
            }
        }

        /// <summary>
        /// Load an mtl file.
        /// </summary>
        /// <param name="objMaterialPath">mtl file path.</param>
        private void LoadMaterial(string objMaterialPath)
        {
            if (!File.Exists(objMaterialPath))
            {
                throw new ArgumentException("File not found!", objMaterialPath);
            }

            var objMaterialFolder = Path.GetDirectoryName(objMaterialPath) ?? string.Empty;
            var streamReader = new StreamReader(objMaterialPath);
            string lastMaterialName = null;
            string line = null;

            while ((line = streamReader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line) || line[0] == '#')
                {
                    continue;
                }
                line = line.Trim();
                string[] s = Regex.Split(line, "\\s+");
                if (s[0] == "#")
                {
                    continue;
                }
                if (s.Length < 2)
                {
                    Console.WriteLine("No arguments for this line: " + line + ".");
                    continue;
                }
                if (s[0] == "newmtl" && s.Length >= 2)
                {
                    #region new material
                    string materialName = string.Join(" ", s.Skip(1));
                    lastMaterialName = materialName;
                    materials[lastMaterialName] = new Material(lastMaterialName);
                    #endregion
                }
                else if ((s[0] == "Ka" || s[0] == "Kd" || s[0] == "Ks") && s.Length >= 4)
                {
                    #region color
                    if (lastMaterialName == null || !materials.ContainsKey(lastMaterialName))
                    {
                        Console.WriteLine("Cannot found material: " + line + ".");
                        continue;
                    }
                    Material material = materials[lastMaterialName];
                    double r, g, b;
                    if (double.TryParse(s[1], out r) && double.TryParse(s[2], out g) && double.TryParse(s[3], out b))
                    {
                        if (s[0] == "Ka")
                        {
                            material.AmbientColor = new Color(r, g, b);
                        }
                        else if (s[0] == "Kd")
                        {
                            material.DiffuseColor = new Color(r, g, b);
                        }
                        else if (s[0] == "Ks")
                        {
                            material.SpecularColor = new Color(r, g, b);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Cannot parse color line: " + line + ".");
                    }
                    #endregion
                }
                else if (s[0] == "Ns" && s.Length >= 2)
                {
                    #region shininess
                    if (lastMaterialName == null || !materials.ContainsKey(lastMaterialName))
                    {
                        Console.WriteLine("Cannot found material: " + line + ".");
                        continue;
                    }
                    Material material = materials[lastMaterialName];
                    double ns;
                    if (double.TryParse(s[1], out ns))
                    {
                        material.Shininess = ns;
                    }
                    else
                    {
                        Console.WriteLine("Cannot parse shininess line: " + line + ".");
                    }
                    #endregion
                }
                else if ((s[0] == "d" || s[0] == "Tr") && s.Length >= 2)
                {
                    #region transparency
                    if (lastMaterialName == null || !materials.ContainsKey(lastMaterialName))
                    {
                        Console.WriteLine("Cannot found material: " + line + ".");
                        continue;
                    }
                    Material material = materials[lastMaterialName];
                    double transparency;
                    if (double.TryParse(s[1], out transparency))
                    {
                        material.Transparency = transparency;
                    }
                    else
                    {
                        Console.WriteLine("Cannot parse transparency line: " + line + ".");
                    }
                    #endregion
                }
                else if (s[0] == "illum" && s.Length >= 2)
                {
                    #region illumination
                    if (lastMaterialName == null || !materials.ContainsKey(lastMaterialName))
                    {
                        Console.WriteLine("Cannot found material: " + line + ".");
                        continue;
                    }
                    Material material = materials[lastMaterialName];
                    int illum;
                    if (int.TryParse(s[1], out illum))
                    {
                        material.Illumination = illum;
                    }
                    else
                    {
                        Console.WriteLine("Cannot parse illum line: " + line + ".");
                    }
                    #endregion
                }
                else if (s[0].StartsWith("map") && s.Length >= 2)
                {
                    #region texture
                    if (lastMaterialName == null || !materials.ContainsKey(lastMaterialName))
                    {
                        Console.WriteLine("Cannot found material: " + line + ".");
                        continue;
                    }
                    Material material = materials[lastMaterialName];
                    // TODO: skip option parameters
                    string texturePath = Path.Combine(objMaterialFolder, s[s.Length - 1]);
                    if (s[0] == "map_Ka")
                    {
                        material.AmbientTexture = new ImageTexture(texturePath);
                    }
                    else if (s[0] == "map_Kd")
                    {
                        material.DiffuseTexture = new ImageTexture(texturePath);
                    }
                    else if (s[0] == "map_Ks")
                    {
                        material.SpecularTexture = new ImageTexture(texturePath);
                    }
                    #endregion
                }
                else if (s[0] == "Ni")
                {
                }
                else
                {
                    Console.WriteLine("Unknown line: " + line + "\n");
                }
            }
        }

        private void GetTriangles()
        {
            foreach (Object o in objects.Values)
            {
                foreach (Face face in o.Faces)
                {
                    if (face.FaceItems.Count == 3)
                    {
                        Vector3 a = vertices[face.FaceItems[0].VertexIndex];
                        Vector3 b = vertices[face.FaceItems[1].VertexIndex];
                        Vector3 c = vertices[face.FaceItems[2].VertexIndex];
                        int n1Index = face.FaceItems[0].NormalIndex;
                        int n2Index = face.FaceItems[1].NormalIndex;
                        int n3Index = face.FaceItems[2].NormalIndex;
                        Triangle triangle;
                        if (n1Index >= 0 && n2Index >= 0 && n3Index >= 0)
                        {
                            Vector3 n1 = normals[n1Index];
                            Vector3 n2 = normals[n2Index];
                            Vector3 n3 = normals[n3Index];
                            triangle = new Triangle(a, n1, b, n2, c, n3, defaultMaterial);
                            // materials[face.MaterialName]
                        }
                        else
                        {
                            triangle = new Triangle(a, b, c, defaultMaterial);
                        }
                        triangles.Add(triangle);
                    }
                    else
                    {
                        Console.WriteLine("Not a triangle face.");
                    }
                }
            }
        }
    }
}
