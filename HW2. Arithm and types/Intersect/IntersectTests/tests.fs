namespace Tests

open NUnit.Framework
open Intersect


[<TestFixture>]
module IntersectTests = 
    let mutable set1, set2, expResult, result = NoPoint, NoPoint, NoPoint, NoPoint
    
    let mutable message = ""

    let testSets() = 
        result <- intersect set1 set2
        message <- sprintf "[Intersect of:]\n%A\n[and]\n%A;\n[Expected]\n%A,\n[got]\n%A" set1 set2 expResult result
        Assert.AreEqual(result, expResult, message)

    [<Test>]
    let PxP1() = 
        set1 <- Point(0.0, 0.0)
        set2 <- Point(0.0, 0.0)
        expResult <- Point(0.0, 0.0)
        testSets()
    
    [<Test>]
    let PxP2() = 
        set1 <- Point(0.0, 1.0)
        set2 <- Point(0.0, 2.0)
        expResult <- NoPoint
        testSets()

    [<Test>]
    let LxL() =      
        set1 <- Line(0.0, 2.0)
        set2 <- Line(0.0, -2.0)
        expResult <- NoPoint
        testSets()
    
    [<Test>]
    let LxVL() =      
        set1 <- Line(6.0, 2.0)
        set2 <- VerticalLine 5.0
        expResult <- Point(5.0, 32.0)
        testSets()
    
    [<Test>]
    let LxLS() = 
        set1 <- Line(10.0, 3.0)
        set2 <- LineSegment((-3.0, -5.0), (3.0, 5.0))
        expResult <- Point(-0.36, -0.6)
        testSets()
    
    [<Test>]
    let LSxLS1() = 
        set1 <- LineSegment((-3.0, -5.0), (3.0, 5.0))
        set2 <- LineSegment((-3.0, -5.0), (3.0, 5.0))
        expResult <- LineSegment((-3.0, -5.0), (3.0, 5.0))
        testSets()

    [<Test>]
    let LSxLS2() = 
        set1 <- LineSegment((0.0, 0.0), (1.0, 1.0))
        set2 <- LineSegment((0.5, 0.5), (1.0, 1.0))
        expResult <- LineSegment((0.5, 0.5), (1.0, 1.0))
        testSets()
    
    [<Test>]
    let LSxLS3() = 
        set1 <- LineSegment((0.0, 0.0), (1.0, 1.0))
        set2 <- LineSegment((0.5, 0.5), (1.0, 1.0))
        expResult <- LineSegment((0.5, 0.5), (1.0, 1.0))
        testSets()
    
    [<Test>]
    let LSxLSxP() =      
        set1 <- Intersect(LineSegment((0.0, 0.0), (0.0, 1.0)), LineSegment((0.0, 1.0), (2.0, 3.0)))
        set2 <- Point(0.0, 1.0)
        expResult <- Point(0.0, 1.0)
        testSets()
    
    [<Test>]
    let LSxLSxPxNP() =      
        set1 <- Intersect(LineSegment((0.0, 0.0), (0.0, 1.0)), LineSegment((0.0, 1.0), (2.0, 3.0)))
        set2 <- Intersect(Point(0.0, 1.0), NoPoint)
        expResult <- NoPoint
        testSets()
        