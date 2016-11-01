# Redux for C#.NET 2.0

I made this piece of code because those redux libraries just don't support .NET 2.0. If you are in the same situation as I was, this piece of code might shed some light on your project.

If you don't know what Redux is ? Educate yourself with [http://redux.js.org](http://redux.js.org), Yes! originally it was designed for javascript web development. Many people found this too good to be kept only in the js world, including me.

I don't intend for this to be a full-fledged redux library, I developed this just for a proof of concept that redux is really compatible with .NET 2.0. Feel free tailor the code to suit your need :D

## What's included

1. **Libs**
	1. class **Act**, every action should extend this class
	2. class **State**, every state should extend this class
	3. class **Store**, every store should extend this class
	4. class **Reducer**, every reducer should extend this class
		* each methods in classes extending this class, should be Reducer functions in the form of `public <StateClass> Reduce(<StateClass> s, <ActClass> act) {..}`
	5. class **ReduceRunner**, create an instance of this class and use it to run reduce functions (keeping the program responsive while reducing).
2. **Examples**
	1. Calculator program

	
## Examples

### Calucalator

This should be an oversimplified calculator application supporting only a plus operation, and have no UI, just for demonstration purposes.

All the actions for this application are defined in the file `calculator/act.cs`

```
public class ActPlus : Act { public int a; public int b; }
public class ActClear : Act { }
```

And now our state `calculator/state.cs`

```
public class CalculatorState : State<CalculatorState>
{
    public bool has_result = false;
    public int result;
}
```

Reducer functions should be defined in `calculator/reducer.cs`

```
public class CalculatorReducer : Reducer<CalculatorState, CalculatorStore>
{
    public CalculatorReducer(CalculatorStore store) : base(store)
    {
    }

    public CalculatorState Reduce(CalculatorState s, ActClear act)
    {
        s = s.Clone();
        s.has_result = false;
        s.result = default(int);
        return s;
    }

    public CalculatorState Reduce(CalculatorState s, ActPlus act)
    {
        s = s.Clone();
        s.has_result = true;
        s.result = act.a + act.b;
        return s;
    }
}
```

The store is in `calculator/store.cs`

```
public class CalculatorStore : Store<CalculatorState, CalculatorStore, CalculatorReducer, CalculatorStore>
{
    public CalculatorStore(ReduceRunner runner) : base(runner)
    {
         
    }

    public override void Dispose()
    {
        // nothing to get rid of
    }

    public override bool IsLoading()
    {
        // we don't use this feature yet
        return false;
    }

    public ManualResetEvent Plus(int a, int b)
    {
        return Dispatch(new ActPlus { a = a, b = b });
    }

    public ManualResetEvent Clear()
    {
        return Dispatch(new ActClear());
    }

}
```

file `calculator/god.cs`, everything combined here.

```
public class God
{
    public CalculatorStore store;
    ReduceRunner runner;
    
    public God()
    {
        // create a reducer runner, and set it run
        runner = new ReduceRunner();
        store = new CalculatorStore(runner);
        runner.SetUp(store.InvokeReduce, ReduceEnd);
    }

    public void ReduceEnd()
    {
        // everytime reduce finishes this runs
        Console.WriteLine("reduce end");
        store.GetState().Dump(); // dumping state to the stdout
    }
}
```

file `Program.cs`, as of C# .NET program everything starts here.

```
class Program
{
    static void Main(string[] args)
    {
        God god = new God();
        god.store.Plus(1, 2).WaitOne(); // waits until this action is reduced
        god.store.Clear();
    }
}
```

And here is the output

```
enqueing action: calculator.ActPlusreducing: calculator.ActPlusreduce endhas_result:True; result:3;enqueing action: calculator.ActClearreducing: calculator.ActClearreduce endhas_result:False; result:0;
```

We can see that the state changes according to an action being reduced. If you are looking for a more realistic application, such as a GUI one, you are free to dig deeper to the gui-example included in this repo!
