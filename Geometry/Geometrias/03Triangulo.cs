using System;

namespace Geometry.Geometrias
{
    [Serializable]
    /// <summary>
    /// Triángulo definido por los tres vértices
    /// </summary>
    public class Triangulo
    {
        /// <summary>
        /// Primer vértice
        /// </summary>
        public Punto3D P1;

        /// <summary>
        /// Segundo vértice
        /// </summary>
        public Punto3D P2;

        /// <summary>
        /// Tercer vértice
        /// </summary>
        public Punto3D P3;

        /// <summary>
        /// Inicializa un triángulo con los vértices en el origen de coordenadas
        /// </summary>
        public Triangulo()
        {
            P1 = new Punto3D();
            P2 = new Punto3D();
            P3 = new Punto3D();
        }

        /// <summary>
        /// Inicializa un triángulo con los vértices P1, P2, y P3
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        public Triangulo(Punto3D p1, Punto3D p2, Punto3D p3)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
        }

        /// <summary>
        /// Retorna -1 o 1 en finción del sentido del triángulo
        /// </summary>
        public int Sentido
        {
            get
            {
                return Analisis.AnalisisGeometrico.SentidoTriangulo(P1, P2, P3);
            }
        }
        
        public void OrdenarVertices(bool SentidoHorario = false)
        {
            Triangulo temp = Analisis.AnalisisGeometrico.OrdenarVertices(this, SentidoHorario);
            this.P1 = temp.P1;
            this.P2 = temp.P2;
            this.P3 = temp.P3;
        }
        /// <summary>
        /// Retorna si el punto indicado está dentro del triángulo
        /// </summary>
        /// <param name="PTest"></param>
        /// <returns></returns>
        public bool Contiene(Punto3D PTest)
        {
            return Analisis.AnalisisGeometrico.PuntoEnTriangulo(PTest, P1, P2, P3);
        }

        public Poligono ToPoligono()
        {
            Poligono ResPol = new Poligono();
            ResPol.Vertices.Add(P1);
            ResPol.Vertices.Add(P2);
            ResPol.Vertices.Add(P3);

            return ResPol;
        }
    }
}
