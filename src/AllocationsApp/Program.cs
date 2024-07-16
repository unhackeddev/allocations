
using System.Buffers;
using System.Runtime.InteropServices;
using System.Text;
// using BenchmarkDotNet.Attributes;
// using BenchmarkDotNet.Running;


BenchmarkRunner.Run<Test_AllocationsArray>();
//BenchmarkRunner.Run<Test_AllocationsList>();
//BenchmarkRunner.Run<Test_AllocationsReadList>();

//[MemoryDiagnoser]
public class Test_AllocationsArray
{
//    [Benchmark(Baseline = true)]
    public string UsingArray()
    {
        byte[] textoAsBytes = Encoding.ASCII.GetBytes("texto");
        string resultado = System.Convert.ToBase64String(textoAsBytes);
        return resultado;
    }

 //   [Benchmark]
    public string UsingArrayPool()
    {
        var texto = "texto";
        int byteCount = Encoding.ASCII.GetByteCount(texto);
        var arrayPool = ArrayPool<byte>.Shared.Rent(byteCount);
        try
        {
            Encoding.ASCII.GetBytes(texto, arrayPool);
            return Convert.ToBase64String(arrayPool);
        }
        finally
        {
           ArrayPool<byte>.Shared.Return(arrayPool);
        }
    }

 //   [Benchmark]
    public string UsingStackalloc()
    {
        var texto = "teste";
        int byteCount = Encoding.ASCII.GetByteCount(texto);
        Span<byte> textoAsBytes = stackalloc byte[byteCount];
        Encoding.ASCII.GetBytes(texto, textoAsBytes);
        return Convert.ToBase64String(textoAsBytes);
    }
}

//[RankColumn]
//[MemoryDiagnoser]
public class Test_AllocationsList
{
  //  [Benchmark(Baseline = true)]
    public void ListWithoutCapacity()
    {
        List<int> items = new List<int>();
        for(int i = 0; i < 100_000; i++)
        {
            items.Add(i);
        }
    }
    
 //   [Benchmark()]
    public void ListWithCapacity()
    {
        const int totalItems = 100_000;
        List<int> items = new List<int>(totalItems);
        for(int i = 0; i < totalItems; i++)
        {
            items.Add(i);
        }
    }
    
  //  [Benchmark()]
    public void ListArrayPool()
    {
        const int totalItems = 100_000;
        var arrayPool = ArrayPool<int>.Shared.Rent(totalItems);
        for(int i = 0; i <= totalItems + 100_000; i++)
        {
            arrayPool[i] = i;
        }
        Array.Clear(arrayPool, 0, totalItems);
        ArrayPool<int>.Shared.Return(arrayPool);
    }
}

//[RankColumn]
//[MemoryDiagnoser]
public class Test_AllocationsReadList
{
    const int totalItems = 100_000;
    private List<int> items = new List<int>(totalItems);

  //  [GlobalSetup]
    public void InicializaLista()
    {
        for (int i = 0; i < totalItems; i++)
        {
            items.Add(i);
        }
    }
    
  //  [Benchmark(Baseline = true)]
    public void ReadFor()
    {
        int soma = 0;
        for(int i = 0; i < totalItems; i++)
        {
            soma += items[i];
        }
    }
    
   // [Benchmark()]
    public void ReadForEach()
    {
        int soma = 0;
        foreach (var i in items)
        {
            soma += i;
        }
    }
    
   // [Benchmark()]
    public void ReadSpan()
    {
        int soma = 0;
        Span<int> spanItems = CollectionsMarshal.AsSpan(items);
        foreach (var i in spanItems)
        {
            soma += i;
        }
    }
}

