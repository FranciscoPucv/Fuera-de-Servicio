using UnityEngine;

public static class EfectosManager
{
   
    public static float multiplicadorMargenFrenado = 1.0f;  
    public static float probabilidadBasuraPoleas = 1.0f;  
    public static float factorAparicionRayones = 1.0f;    
    public static int stockAceite = 0;
    public static int stockKitsLimpieza = 0;
    public static int stockLijaBarniz = 0;
    public static int stockRepuestosFreno = 0;
    public static int stockCristales = 0;


    public static void AplicarEfecto(string idItem)
    {
        switch (idItem.ToLower())
        {
            // Mejoras Permanentes
            case "frenos":
                multiplicadorMargenFrenado = 1.5f;
                Debug.Log("[Efectos] Margen de frenado aumentado.");
                break;

            case "malla":
                probabilidadBasuraPoleas = 0.3f;
                Debug.Log("[Efectos] Malla instalada: riesgo de basura reducido al 30%.");
                break;

            case "letrero":
                factorAparicionRayones = 0.5f;
                Debug.Log("[Efectos] Letrero informativo activo: vandalismo reducido.");
                break;

            // Consumibles acumulables
            case "aceite":
                stockAceite++;
                Debug.Log($"[Inventario] Aceite disponible: {stockAceite}");
                break;

            case "kit_limpieza":
                stockKitsLimpieza++;
                Debug.Log($"[Inventario] Kits de limpieza disponibles: {stockKitsLimpieza}");
                break;

            case "lija_barniz":
                stockLijaBarniz++;
                Debug.Log($"[Inventario] Lija y barniz disponibles: {stockLijaBarniz}");
                break;

            case "repuesto":
                stockRepuestosFreno++;
                Debug.Log($"[Inventario] Repuestos de freno: {stockRepuestosFreno}");
                break;

            case "cristal":
                stockCristales++;
                Debug.Log($"[Inventario] Cristales de repuesto: {stockCristales}");
                break;
        }
    }

    public static bool ConsumirAceite()
    {
        if (stockAceite > 0) { stockAceite--; return true; }
        return false;
    }

    public static bool ConsumirKitLimpieza()
    {
        if (stockKitsLimpieza > 0) { stockKitsLimpieza--; return true; }
        return false;
    }
}