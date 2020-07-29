# Of

## How should instances be created?

```cs
class Address : Of<string> {}

var address = new Address { Value = "1 Microsoft Way" };
Console.WriteLine(address); // prints "1 Microsoft Way"
```

it would be nicer if you could just do

```cs
var address = new Address("1 Microsoft Way");
```

but, then you have to define the type with boilerplate for the constructor.

The best alternative using helper methods I've gotten to work is:

```cs
var address = Of.Create("1 Microsoft Way").To<Address>();
```

Do that by making `Of<T>` concrete with a protected constructor, and add the static `Of` type and the `To<TOf>` method.

So yeah, I don't think there's a way out of this in that direction.

Defining the constructor to remove the duplication is boilerplate, but pretty intuitive:

```cs
class Address : Of<string>
{
    public Address(string value) => Value = value;
}
```

This also gives you an easy way to hook in custom validation for the value, which is desirable.

## Should the type be immutable?

It's tempting to require the `Of<T>` constructor to take the value for `Value` and require subtypes to pass it in.
Then, all `Of` types will be immutable.

Unfortunately, this complicates the idiom considerably:

```cs
class Address : Of<string> { public Address(string x) : base(x) { } }
```

Immutability is great, but I don't think it provides enough of a benefit to make the API this ugly.

I think people will gravitate towards defining subtypes like this anyway:

```cs
class Address : Of<string>
{
    public Address(string value) => Value = value;
}
```

but, offering choice is Godlike.

## Conversion operators

Another question is whether `Of` should have

```cs
public static implicit operator T(Of<T> of) => of.Value;
```

First of all, an explicit conversion operator adds no value (pun intended).
The implicit operator saves you from having to put `.Value` to pass the instance to, say, an argument of type `T`.
It makes it feel like `Of<T>` is a little more like a subtype of `T`, which is conceptually what it is.

I think implicit conversions are kind of spooky, though, and it wouldn't be a breaking change to add that in the future.

**Update from the future:** when deserializing JSON, there's this limitation:

```cs
// throws InvalidCastException
JsonSerializer.Deserialize<Address>("\"1 Microsoft Way\"")
```

The workaround is to do something like this:

```cs
(Address)JsonSerializer.Deserialize<Of<string>>("\"1 Microsoft Way\"")
```

This requires an explicit operator to cast `Of<T>` to `Of<U>`.


(Of course, you could still construct the values normally, but that would be really clunky.)

I'm also just going to go ahead and add the implicit operator.
I've found myself typing `.Value` a lot to pass my `Of<T>` subtypes to methods from code I don't control.

## Any considerations to enable nullable references in a later version?

No, just put

```cs
public T Value { get; set; } = default;
```