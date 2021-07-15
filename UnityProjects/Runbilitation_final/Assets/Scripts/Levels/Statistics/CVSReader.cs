using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

/*Questa funzione "parsifica" (legge e traduce) un file con valori separati da una virgola (CSV, comma separeted values)
  trasforma i valori letti in interi (int) o numeri reali (float) e li restituisce in una lista (struttura dati List)

    Versione originale presa da: https://bravenewmethod.com/2014/09/13/lightweight-csv-reader-for-unity/
    
*/

public class CSVReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))"; //Qui vengono definiti i delimitatori tra un valore ed un altro. La sintassi utilizzata è quella delle espressioni regolari, abbastanza complessa e al di fuori degli argomenti del corso.
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r"; // Qui vengono definiti i delimitatori di riga
    static char[] TRIM_CHARS = { '\"' }; //Qui i caratteri da ignorare durante la lettura del file (in questo caso le " )

    //Dichiarazione ed implementazione del metodo "Read" che effettua le operazioni necessarie
    // è necessario passare alla funzione un nome file (il file CSV appunto) affinchè sappia cosa leggere
    public static List<Dictionary<string, object>> Read(string file) 
    {
        var list = new List<Dictionary<string, object>>(); //viene preparata la lista da restituire alla fine della procedure. Ogni elemento della lista è un oggetto di tipo Dictionary che associa un nome (string) ad ogni valore non ancora tradotto (object)
        TextAsset data = Resources.Load(file) as TextAsset; //Viene caricato il file CSV che abbiamo indicato e interpretato come testo
 
        var lines = Regex.Split(data.text, LINE_SPLIT_RE); //Il file letto prima viene diviso in linee, sulla base dei delimitatori definiti in LINE_SPLIT_RE

        if (lines.Length <= 1) return list; //Se non ci sono linee, ovvero se il file è vuoto, esci dalla funzione e restituisci un valore (-1) che indichi l'errore

        var header = Regex.Split(lines[0], SPLIT_RE); //Legge la linea 0esima, ovvero la prima della struttura lines, considerandola quella coi nomi delle colonne che vengono separati usando i delimitatori in SPLIT_RE.
        for (var i = 1; i < lines.Length; i++) //partendo dalla linea successiva, la 1, esegue una serie di operazioni su tutte le altre in ordine.
        {

            var values = Regex.Split(lines[i], SPLIT_RE); //Suddivide i valori della linea i-esima in base ai delimitatori in SPLIT_RE
            if (values.Length == 0 || values[0] == "") continue; //se per caso ha incontrato una linea vuota, passa alla successiva

            var entry = new Dictionary<string, object>(); //Crea un oggetto vuoto da aggiungere alla lista restituita alla fine
            for (var j = 0; j < header.Length && j < values.Length; j++) //Considera i valori presenti in una linea, uno alla volta, basandosi sul numero di colonne rilevate leggendo la riga di intestazione e (&&) sul numero di valori effettivamente presenti in questa linea.
            {
                string value = values[j]; //legge il valore j-esimo della linea. Il valore letto adesso è una string, essendo quello aperto un file di testo, quindi composto da stringhe.
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", ""); //elimina eventuali "" presenti 
                object finalvalue = value; //Crea l'oggetto da inserire nella variabile entry
                int n; //crea un intero n, nel caso il valore letto sia un intero
                float f; //crea il numero reale n, nel caso il valore letto sia un numero decimale
                if (int.TryParse(value, out n)) //prova a interpretare il valore value come intero e a salvarlo nella variabile n...
                {
                    finalvalue = n; //...se ci riesce, finalvalue diventa uguale a questo valore
                }
                else if (float.TryParse(value, out f)) // se non ci riesce (else) allora prova ad interpretarlo come float...
                {
                    finalvalue = f; //se ci riesce, finalvalue diventa uguale ad f
                }
                entry[header[j]] = finalvalue; //ora può impostare tutto quello che serve nella variabile entry <(il nome della colonna relativa al valore appena interpretato), finalvalue>
            }
            list.Add(entry); //aggiunge il valore letto alla nostra lista.
        }
        return list; //quando ha completato l'interpretazione di tutti i valori di tutte le linee del file conclude la procedura restituendo la lista compilata
    }
}