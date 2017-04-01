using System;

public class Triangulacion
{
	public Triangulacion()
	{
	}

    public enum TipoTriangulado
    {
        Avanico,/*https://es.wikipedia.org/wiki/Triangulaci%C3%B3n_en_abanico*/
        Delaunay,/*https://es.wikipedia.org/wiki/Triangulaci%C3%B3n_de_Delaunay*/
        MinimoPeso,/*https://es.wikipedia.org/wiki/Triangulaci%C3%B3n_de_peso_m%C3%ADnimo*/
        Voraz/*https://es.wikipedia.org/wiki/Algoritmo_de_triangulaci%C3%B3n_voraz*/
    }
    public static IList<Point3dCollection> TriangularPoligono(Point3dCollection PerimetroPoligono, TipoTriangulado Metodo = TipoTriangulado.Delaunay)
    {
        IList<Point3dCollection> ResTriangulacion = new List<Point3dCollection>();



        return ResTriangulacion;
    }
}
