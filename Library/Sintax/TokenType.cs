namespace HulkProject;

//Enun que contiene todos los tipos de token del lenguaje
public enum TokenType
{
    //Tokens
    NumberToken,
    PlusToken,
    MinusToken,
    MultiplyToken,
    DivisionToken,
    OpenParenthesisToken,
    CloseParenthesisToken,
    EndOffLineToken,
    NotToken,
    LowerToken,
    BiggerToken,
    LowerOrEqualToken,
    BiggerOrEqualToken,
    NotEqualToken,
    ExponentialToken,
    SingleAndToken,
    SingleOrToken,
    ModuleToken,
    EqualToken,
    SingleEqualToken,
    WrongToken,
    LetToken,
    ArrobaToken,
    InToken,
    StringToken,
    ColonToken,
    ArrowToken,
    Identifier,

    //Keywords
    TrueKeyword,
    FalseKeyword,
    IfKeyword,
    ElseKeyword,
    FunctionKeyword,

    // Expressions
    LiteralExpression,
    BinaryExpression,
    UnaryExpression,
    ParenthesizedExpression,
    IfElseExpression,
}
