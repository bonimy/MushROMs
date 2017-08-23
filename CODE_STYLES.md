# C# Coding Conventions
A lot of the content in this document is taken directly from Microsoft's [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions).

Coding conventions serve the following purposes:
- They create a consistent look to the code, so that readers can focus on content, not layout.
- They enable readers to understand the code more quickly by making assumptions based on previous experience.
- They facilitate copying, changing, and maintaining the code.
- They demonstrate C# best practices.


## Table of Contents
 - [Naming Conventions](#naming-conventions)
 - [Layout Conventions](#layout-conventions)
 - [Commenting Conventions](#commenting-conventions)
 - [Language Guidelines](#language-guidelines)
     - [General Code Style](#general-code-style)
     - [Indentation](#indentation)
     - [New Lines](#new-lines)
     - [Spacing](#spacing)
     - [String Data Type](#string-data-type)
     - [Implicitly Typed Local Variables](#implicitly-typed-local-variables)
     - [Unsigned Data Type](#unsigned-data-type)
     - [Arrays](#arrays)
     - [Delegates](#delegates)
     - [try-catch and using Statements in Exception Handling](#try-catch-and-using-statements-in-exception-handling)
     - [&& and || Operators](#-and--operators)
     - [New operator](#new-operator)
     - [Event Handling](#event-handling)
     - [Static Members](#static-members)
     - [LINQ Queries](#linq-queries)

## Naming Conventions
- In short examples that do not include [using directives](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/using-directive), use namespace qualifications.
- If you know that a namespace is imported by default in a project, you do not have to fully qualify the names from that namespace.
- Qualified names can be broken after a dot `.` if they are too long for a single line, as shown in the following example.
```C#
var currentPerformanceCounterCategory = new System.Diagnostics.
    PerformanceCounterCategory();
```
- You do not have to change the names of objects that were created by using the Visual Studio designer tools (and are not used outside of the designer) to make them fit other guidelines.
- All interfaces must begin with `I`.
- All types must be `PascalCase`.
- All locals, parameters, and field members must be `camelCase`.
- All field members must begin with an underscore `_`.
- Field members can only be private or internal. In place of public or protected fields, use `get`\\`set` accessors.

## Layout Conventions
 Good layout uses formatting to emphasize the structure of your code and to make the code easier to read.
- Use smart indenting, four-character indents, and tabs saved as spaces.
- Write only one statement per line.
- Write only one declaration per line.
- If continuation lines are not indented automatically, indent them one tab stop (four spaces).
- Add at least one blank line between method definitions and property definitions.
- Use parentheses to make clauses in an expression apparent, as shown in the following code.
```C#
if ((val1 > val2) && (val1 > val3))
{
    // Take appropriate action.
}
```

## Commenting Conventions
- Place the comment on a separate line, not at the end of a line of code.
- Begin comment text with an uppercase letter.
- End comment text with a period.
- Insert one space between the comment delimiter '//' and the comment text, as shown in the following example.
```C#
// The following declaration creates a query. It does not run
// the query.
```
- Do not create formatted blocks of asterisks around comments.

## Language Guidelines
The following sections describe practices that you should follow when writing code.

### General Code Style
- Do _not_ qualify field, property, method, or event access with `this.`.
- For locals, parameters, and members, use the predefined type (e.g. `int`, `string`, etc.),
- For member access expressions, use the framework type (e.g. `System.Int32.MaxValue`).
- Uses braces for single-line code blocks
```C#
if (test)
{
    Display();
}
```
- Use object and collection initializers.
- Use pattern matching in `is` and `as` statements (.NET 4.7 required).
- Use explicit tuple names.
- Use simple default expressions.
- Use expression bodies for single-line properties, indexers, and accessors.
- Do _not_ use expression bodies for methods, constructors, or operators.
- Use inlined variable declaration (.NET 4.7 required).
- Use `?` and `??` for `null` checking.

### Indentation
- Always indent block and case contents.
- Do _not_ indent open and close braces or case labels.
- Place labels one indent less than current.

### New Lines
- Place open and close braces on new lines.
- Place `else`, `catch`, and `finally` on new lines.
- Place members in object initializers and anonymous types on new line.
- Place query expression clauses on new line.

### Spacing
- Do _not_ insert space between method name and its opening parenthesis, within parameter/argument list parentheses, or within empty parameter/argument list parentheses.
- Insert space after keywords in control flow statements (`for`, `foreach`, `if`, etc.).
- Do not insert space within parentheses of expressions, type casts, or control flow statements.
- Do not insert space after a cast.
- Extra space in declaration statements is okay for alignment purposes.
- Do not insert space before or within square brackets.
- Insert space before and after colon delimeters.
- Insert space after (but not before) comma delimeters and semicolon delimeters in `for` statements.
- Insert space before and after binary operators.

### String Data Type
- Use the `+` operator to concatenate short strings, as shown in the following code.
```C#
string displayName = nameList[n].LastName + ", " + nameList[n].FirstName;
```
- To append strings in loops, especially when you are working with large amounts of text, use a [StringBuilder](https://docs.microsoft.com/en-us/dotnet/api/system.text.stringbuilder?view=netframework-4.7) object.
```C#
var phrase = "lalalalalalalalalalalalalalalalalalalalalalalalalalalalalala";
var manyPhrases = new StringBuilder();
for (var i = 0; i < 10000; i++)
{
    manyPhrases.Append(phrase);
}
//Console.WriteLine("tra" + manyPhrases);
```

### Implicitly Typed Local Variables
Use [implicit typing](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/implicitly-typed-local-variables) for local variables when the type of the variable is obvious from the right side of the assignment, or when the precise type is not important.
```C#
// When the type of a variable is clear from the context, use var 
// in the declaration.
var var1 = "This is clearly a string.";
var var2 = 27;
var var3 = Convert.ToInt32(Console.ReadLine());
```
- Do not use [var](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/var) when the type is not apparent from the right side of the assignment.
```C#
// When the type of a variable is not clear from the context, use an
// explicit type.
int var4 = ExampleClass.ResultSoFar();
```
- Do not rely on the variable name to specify the type of the variable. It might not be correct.
```C#
// Naming the following variable inputInt is misleading. 
// It is a string.
var inputInt = Console.ReadLine();
Console.WriteLine(inputInt);
```
- Avoid the use of `var` in place of [dynamic](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/dynamic).

- Use implicit typing to determine the type of the loop variable in [for](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/for) and [foreach](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/foreach-in) loops.

The following example uses implicit typing in a `for` statement.
```C#
var syllable = "ha";
var laugh = "";
for (var i = 0; i < 10; i++)
{
    laugh += syllable;
    Console.WriteLine(laugh);
}
```
The following example uses implicit typing in a `foreach` statement.
```C#
foreach (var ch in laugh)
{
    if (ch == 'h')
        Console.Write("H");
    else
        Console.Write(ch);
}
Console.WriteLine();
```

### Unsigned Data Type
- In general, use `int` rather than unsigned types. The use of `int` is common throughout C#, and it is easier to interact with other libraries when you use `int`.

### Arrays
- Use the concise syntax when you initialize arrays on the declaration line.
```C#
// Preferred syntax. Note that you cannot use var here instead of string[].
string[] vowels1 = { "a", "e", "i", "o", "u" };


// If you use explicit instantiation, you can use var.
var vowels2 = new string[] { "a", "e", "i", "o", "u" };

// If you specify an array size, you must initialize the elements one at a time.
var vowels3 = new string[5];
vowels3[0] = "a";
vowels3[1] = "e";
// And so on.
```

### Delegates
- Use the concise syntax to create instances of a delegate type.
```C#
// First, in class Program, define the delegate type and a method that  
// has a matching signature.

// Define the type.
public delegate void Del(string message);

// Define a method that has a matching signature.
public static void DelMethod(string str)
{
    Console.WriteLine("DelMethod argument: {0}", str);
}
```
```C#
// In the Main method, create an instance of Del.

// Preferred: Create an instance of Del by using condensed syntax.
Del exampleDel2 = DelMethod;

// The following declaration uses the full syntax.
Del exampleDel1 = new Del(DelMethod);
```
### try-catch and using Statements in Exception Handling
- Use a [try-catch](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/try-catch) statement for most exception handling.
```C#
static string GetValueFromArray(string[] array, int index)
{
    try
    {
        return array[index];
    }
    catch (System.IndexOutOfRangeException ex)
    {
        Console.WriteLine("Index is out of range: {0}", index);
        throw;
    }
}
```
- Simplify your code by using the C# [using statement](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/using-statement). If you have a [try-finally](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/try-finally) statement in which the only code in the `finally` block is a call to the [Dispose](https://docs.microsoft.com/en-us/dotnet/api/system.idisposable.dispose?view=netframework-4.7) method, use a `using` statement instead.
```C#
// This try-finally statement only calls Dispose in the finally block.
Font font1 = new Font("Arial", 10.0f);
try
{
    byte charset = font1.GdiCharSet;
}
finally
{
    if (font1 != null)
    {
        ((IDisposable)font1).Dispose();
    }
}


// You can do the same thing with a using statement.
using (Font font2 = new Font("Arial", 10.0f))
{
    byte charset = font2.GdiCharSet;
}
```

### && and || Operators
- To avoid exceptions and increase performance by skipping unnecessary comparisons, use [&&](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/conditional-and-operator) instead of [&](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/and-operator) and [||](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/conditional-or-operator) instead of [|](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/or-operator) when you perform comparisons, as shown in the following example.
```C#
Console.Write("Enter a dividend: ");
var dividend = Convert.ToInt32(Console.ReadLine());

Console.Write("Enter a divisor: ");
var divisor = Convert.ToInt32(Console.ReadLine());

// If the divisor is 0, the second clause in the following condition
// causes a run-time error. The && operator short circuits when the
// first expression is false. That is, it does not evaluate the
// second expression. The & operator evaluates both, and causes 
// a run-time error when divisor is 0.
if ((divisor != 0) && (dividend / divisor > 0))
{
    Console.WriteLine("Quotient: {0}", dividend / divisor);
}
else
{
    Console.WriteLine("Attempted division by 0 ends up here.");
}
```

### New Operator
- Use the concise form of object instantiation, with implicit typing, as shown in the following declaration.
```C#
var instance1 = new ExampleClass();
```
The previous line is equivalent to the following declaration.
```C#
ExampleClass instance2 = new ExampleClass();
```
- Use object initializers to simplify object creation.
```C#
// Object initializer.
var instance3 = new ExampleClass { Name = "Desktop", ID = 37414, 
    Location = "Redmond", Age = 2.3 };

// Default constructor and assignment statements.
var instance4 = new ExampleClass();
instance4.Name = "Desktop";
instance4.ID = 37414;
instance4.Location = "Redmond";
instance4.Age = 2.3;
```

### Event Handling
- If you are defining an event handler that you do not need to remove later, use a lambda expression.
```C#
public Form2()
{
    // You can use a lambda expression to define an event handler.
    this.Click += (s, e) =>
        {
            MessageBox.Show(
                ((MouseEventArgs)e).Location.ToString());
        };
}
```
```C#
// Using a lambda expression shortens the following traditional definition.
public Form1()
{
    this.Click += new EventHandler(Form1_Click);
}

void Form1_Click(object sender, EventArgs e)
{
    MessageBox.Show(((MouseEventArgs)e).Location.ToString());
}
```

### Static Members
- Call [static](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/static) members by using the class name: _ClassName.StaticMember_. This practice makes code more readable by making static access clear.
- Do not qualify a static member defined in a base class with the name of a derived class. While that code compiles, the code readability is misleading, and the code may break in the future if you add a static member with the same name to the derived class.

### LINQ Queries
- Use meaningful names for query variables. The following example uses `seattleCustomers` for customers who are located in Seattle.
```C#
var seattleCustomers = from cust in customers
                       where cust.City == "Seattle"
                       select cust.Name;
`
- Use aliases to make sure that property names of anonymous types are correctly capitalized, using Pascal casing.
```C#
var localDistributors =
    from customer in customers
    join distributor in distributors on customer.City equals distributor.City
    select new { Customer = customer, Distributor = distributor };
```
- Rename properties when the property names in the result would be ambiguous. For example, if your query returns a customer name and a distributor ID, instead of leaving them as `Name` and `ID` in the result, rename them to clarify that `Name` is the name of a customer, and `ID` is the ID of a distributor.
```C#
var localDistributors2 =
    from cust in customers
    join dist in distributors on cust.City equals dist.City
    select new { CustomerName = cust.Name, DistributorID = dist.ID };
```
- Use implicit typing in the declaration of query variables and range variables.
```C#
var seattleCustomers = from cust in customers
                       where cust.City == "Seattle"
                       select cust.Name;
```
- Align query clauses under the from clause, as shown in the previous examples.
- Use [where](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/where-clause) clauses before other query clauses to ensure that later query clauses operate on the reduced, filtered set of data.
```C#
var seattleCustomers2 = from cust in customers
                        where cust.City == "Seattle"
                        orderby cust.Name
                        select cust;
```
- Use multiple `from` clauses instead of a [join](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/join-clause) clause to access inner collections. For example, a collection of `Student` objects might each contain a collection of test scores. When the following query is executed, it returns each score that is over 90, along with the last name of the student who received the score.
```C#
// Use a compound from to access the inner sequence within each element.
var scoreQuery = from student in students
                 from score in student.Scores
                 where score > 90
                 select new { Last = student.LastName, score };
```
