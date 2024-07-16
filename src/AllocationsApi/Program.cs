using System.Collections.Concurrent;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

ConcurrentDictionary<int, string> _concurrentDictionary = new();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


app.MapGet("/exemplo1/{id}", (int id) =>
{
    // int i = id;
    // Func<int> f = () =>
    //  {         
    //      return ++i;
    //  };
    // var j = f();
    // return Results.Ok(value: j);

    #region hide
    int i = id;
    Func<int, int> f = (int i) => 
    {         
        return ++i;
    };
    var j = f(i);
    return Results.Ok(value: j);
    #endregion
});

app.MapGet("/exemplo2/{chave}/{valor}", (int chave, string valor) =>
{
    string retorno;
    // retorno = _concurrentDictionary.GetOrAdd(chave, x => valor);
    // return Results.Ok(retorno);
    #region hide
    retorno = _concurrentDictionary.GetOrAdd(chave, static (k, v) => v, valor);
    return Results.Ok(retorno);
    #endregion
});


app.Run();