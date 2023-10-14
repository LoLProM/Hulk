namespace HulkProject;

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
    PrintKeyword,
}
