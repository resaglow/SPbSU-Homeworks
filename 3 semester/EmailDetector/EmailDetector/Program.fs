open System.Text.RegularExpressions

open NUnit.Framework
open FsUnit

let pattern = "^[a-zA-Z_]((\.)?[\w-_]){0,30}@([\w]+\.)+([a-zA-Z]{2,3}|aero|asia|coop|info|jobs|mobi|museum|name|travel)$"

let checkEmail email = Regex.IsMatch(email, pattern)

[<TestFixture>]
module trueTests =     
    [<Test>]
    let test1() = checkEmail "a@b.cc" |> should be True

    [<Test>]
    let test3() = checkEmail "victor.polozov@mail.ru" |> should be True

    [<Test>]
    let test4() = checkEmail "my@domain.info" |> should be True

    [<Test>]
    let test5() = checkEmail "_.1@mail.com" |> should be True

    [<Test>]
    let test6() = checkEmail "paints_department@hermitage.museum" |> should be True

[<TestFixture>]
module falseTests = 
    [<Test>]
    let test7() = checkEmail "a@b.c" |> should be False

    [<Test>]
    let test8() = checkEmail "a..b@mail.ru" |> should be False

    [<Test>]
    let test9() = checkEmail ".a@mail.ru" |> should be False

    [<Test>]
    let test10() = checkEmail "yo@domain.somedomain" |> should be False

    [<Test>]
    let test11() = checkEmail "1@mail.ru" |> should be False
