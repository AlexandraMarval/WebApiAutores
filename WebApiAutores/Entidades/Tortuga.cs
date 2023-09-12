namespace  WebAPIAutores.Entidades;

public class TortugaGrande : ITortuga
{
    public string Mensaje { get; private set; }
    public int CantidadVueltas { get; set; }

    public void DarVueltas(int vueltas)
    {
        CantidadVueltas = vueltas;
    }

    public string Hablar(string mensaje)
    {
        Mensaje = $"Soy una tortuga grande: {mensaje}";
        return Mensaje;
    }
}

public class TortugaPequeña : ITortuga
{
    public string Mensaje { get; set; }
    public int CantidadVueltas { get; set; }

    public string Hablar(string mensaje)
    {
        Mensaje = $"Soy una tortuga pequeña: + {mensaje}";
        return Mensaje;
    }

    public void DarVueltas(int vueltas)
    {
        throw new NotImplementedException();
    }
}
public interface ITortuga 
{ 
    string Mensaje { get; }
    int CantidadVueltas { get; set; }
    
    string Hablar(string mensaje);  
    void DarVueltas(int vueltas);
}

