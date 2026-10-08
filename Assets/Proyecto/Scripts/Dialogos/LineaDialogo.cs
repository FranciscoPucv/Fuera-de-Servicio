using System;
using System.Collections.Generic;

[Serializable]
public class LineaDialogo
{
    public string hablante;
    public string texto;
    public string imagen;
}

[Serializable]
public class Dialogo
{
    public string id;
    public List<LineaDialogo> lineas;
}

[Serializable]

public class DialogosWrapper
{
    public List<Dialogo> dialogos;  
}