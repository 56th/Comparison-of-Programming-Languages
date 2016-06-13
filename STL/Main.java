import java.io.*;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.*;

class WordFreeq {
    public String   word;
    public Integer  freeq;

    public WordFreeq( String w, int f ) {
        word    = w;
        freeq   = f;
    }

    public String toString() {
        return "[" + word + ", " + freeq + "]";
    }
}

class KeyValuePair<A, B> {
    public A first;
    public B second;
}

public class Main {
    // Fields
    public static ArrayList<String>     words;

    public static String                uniqueWord;
    public static int                   wordsCount;
    public static int                   uniqueCount;

    public static HashSet<String>       wordsToExclude;
    public static TreeMap<String, Integer> freeq;
    public static ArrayList<WordFreeq>  pairs;


    // Methods
    public static void readExcludeWords( String path ) throws IOException {
        byte[] bytes = Files.readAllBytes( Paths.get( path ) );
        String allWords = new String( bytes );
        wordsToExclude = new HashSet<String>( Arrays.asList( allWords.split( "[\n ]" ) ) );
    }

    public static void readText( String path ) throws IOException {
        byte[] bytes = Files.readAllBytes( Paths.get(path) );
        String fullText = new String( bytes );
        words = new ArrayList<String>( Arrays.asList( fullText.split( "[\n ]" ) ) );
    }

    public static void main(String[] args) throws IOException {
        readExcludeWords( "ignore.txt" );
        for ( String s : wordsToExclude ) {
            wordsToExclude.add( s.toLowerCase() );
        }
        readText( "Martin Eden.txt" );

        words.removeAll( wordsToExclude );

        long begin = System.currentTimeMillis();

        // Частота встречи слов
        for ( int i = 0, n = words.size(); i < n; ++i ) {
            if ( freeq.containsKey( words.get(i) ) ) {
                Integer k = freeq.get( words.get(i) );
                freeq.put( words.get( i ), k );
            }
            else {
                freeq.put( words.get( i ), 1 );
            }
        }

        String maxWord = "";

        TreeMap<String, Integer> pairsTree = new TreeMap<String, Integer>();
        for ( int i = 1, n = words.size() - 1; i < n; ++i ) {
            if ( words.get(i) != maxWord && words.get(i+1) != maxWord )
                continue;

            String p = words.get(i) + " " + words.get(i+1);
            if ( pairsTree.containsKey( p ) ) {
                Integer k = pairsTree.get( p );
                pairsTree.put( p, k );
            }
            else
                pairsTree.put( p, 1 );
        }

        pairs.clear();
        for ( Map.Entry<String, Integer> entry : pairsTree.entrySet() ) {
            pairs.add( new WordFreeq( entry.getKey(), entry.getValue() ) );
        }

        Collections.sort( pairs, new Comparator<WordFreeq>() {
            public int compare( WordFreeq s1, WordFreeq s2 ) {
                return s2.freeq - s1.freeq;
            } 
        });

        long end = System.currentTimeMillis() - begin;
        
        // writeAnswer();
        System.out.println( "Time " + end );
        
        // writer.close();
    }
}