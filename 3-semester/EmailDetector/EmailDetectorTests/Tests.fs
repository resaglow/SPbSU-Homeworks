namespace EmailDetectorTests

open NUnit.Framework

open emailDetector

[<TestFixture>]
module trueTests =     
    [<Test>]
    let test1() = Assert.IsTrue(checkEmail "a@b.cc" )

    [<Test>]
    let test3() = Assert.IsTrue(checkEmail "victor.polozov@mail.ru")

    [<Test>]
    let test4() = Assert.IsTrue(checkEmail "my@domain.info")

    [<Test>]
    let test5() = Assert.IsTrue(checkEmail "_.1@mail.com")

    [<Test>]
    let test6() = Assert.IsTrue(checkEmail "paints_department@hermitage.museum")

[<TestFixture>]
module falseTests = 
    [<Test>]
    let test7() = Assert.False(checkEmail "a@b.c")

    [<Test>]
    let test8() = Assert.False(checkEmail "a..b@mail.ru")

    [<Test>]
    let test9() = Assert.False(checkEmail ".a@mail.ru")

    [<Test>]
    let test10() = Assert.False(checkEmail "yo@domain.somedomain")

    [<Test>]
    let test11() = Assert.False(checkEmail "1@mail.ru")
