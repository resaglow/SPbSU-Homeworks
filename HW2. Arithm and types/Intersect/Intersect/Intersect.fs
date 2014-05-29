(*  Artem Lobanov (c) 2014
    Intersect of Points & Lines & Line Segments
   
    Я бы мог разбить intersect на функции но match и так разбивает более менее, 
    а дальше разбивать, как мне кажется, в этом конкретном случае не стоит
*)

module Intersect

let eps = 0.0001

let (===) (a:float) (b:float) = abs (a - b) < eps

[<CustomEquality; NoComparison>]
type Geom = 
    | NoPoint                  
    | Point of float * float   
    | Line  of float * float  
    | VerticalLine of float    
    | LineSegment of (float * float) * (float * float)
    | Intersect of Geom * Geom

    override this.Equals anotherObj =
        match anotherObj with
        | :? Geom as anotherGeom ->
            match this, anotherGeom with 
            | (NoPoint, NoPoint) -> true
            | (Point (c1, c2), Point (c3, c4)) // "c" for coordinate
            | (Line (c1, c2), Line (c3, c4)) -> c1 === c3 && c2 === c4 
            | (VerticalLine x1, VerticalLine x2) -> x1 === x2 
            | (LineSegment ((x1, y1), (x2, y2)), LineSegment ((x3, y3), (x4, y4))) -> x1 === x3 && y1 === y3 && x2 === x4 && y2 === y4
            | _ -> false
        | _ -> false

let getSegmentLine x1 y1 x2 y2 = Line ((y2 - y1) / (x2 - x1), (x2 * y1 - x1 * y2) / (x2 - x1))

let diffSidesPoints p1 p2 center = (p1 <= center && center <= p2) || (p1 >= center && center >= p2)

let rec intersect set1 set2 =
    match (set1, set2) with
    | (NoPoint, _) | (_, NoPoint) -> NoPoint

    | (Point (x1, y1), Point (x2, y2)) -> 
        if x1 === x2 && y1 === y2 then Point (x1, y1) 
        else NoPoint

    | (Point (x, y), Line (a, b)) | (Line (a, b), Point(x, y)) -> 
        if a * x + b === y then Point (x, y) 
        else NoPoint

    | (Point (x, y), VerticalLine verticalX) | (VerticalLine verticalX, Point (x, y)) -> 
        if x === verticalX then Point (x, y) 
        else NoPoint

    | (Point (x, y), LineSegment ((x1, y1), (x2, y2))) 
    | (LineSegment ((x1, y1), (x2, y2)), Point (x, y)) -> 
        if x1 <> x2 then
            let segmentLine = getSegmentLine x1 y1 x2 y2
            let pointLineIntersection = intersect (Point (x, y)) segmentLine

            match pointLineIntersection with
            | Point (interX, interY) -> if diffSidesPoints x1 x2 interX then pointLineIntersection else NoPoint
            | NoPoint -> NoPoint
            | _ -> failwith "Error"

        else if x === x1 then Point (x, y)
        else NoPoint
                                                              

    | (Line (a1, b1), Line (a2, b2)) -> 
        if a1 === a2 then
            if b1 === b2 then Line (a1, b1)
            else NoPoint
        else
            let x = (b2 - b1) / (a1 - a2)
            Point (x, a1 * x + b1)

    | (Line (a, b), VerticalLine verticalX) | (VerticalLine verticalX, Line (a, b)) -> Point (verticalX, a * verticalX + b)

    | (Line (a, b), LineSegment ((x1, y1), (x2, y2))) 
    | (LineSegment ((x1, y1), (x2, y2)), Line (a, b)) -> 
        if x1 <> x2 then // Nonvertical LineSegment
            let segmentLine = getSegmentLine x1 y1 x2 y2
            match segmentLine with
            | Line (segmentA, segmentB) -> 
                if segmentA = a then // On parallel lines
                    if segmentB = b then // On the same line
                        LineSegment ((x1, y1), (x2, y2))
                    else NoPoint // On parallel but not the same
                else
                    let possiblePoint = intersect segmentLine (Line (a, b))
                    intersect possiblePoint (LineSegment ((x1, y1), (x2, y2)))
            | _ -> failwith "Error"
        else // Vertical Linesegment
            if diffSidesPoints y1 y2 (a * x1 + b)
               then Point (x1, a * x1 + b)
            else NoPoint

    | (VerticalLine verticalX1, VerticalLine verticalX2) -> if verticalX1 === verticalX2 then VerticalLine verticalX1 else NoPoint

    | (VerticalLine verticalX, LineSegment ((x1, y1), (x2, y2)))
    | (LineSegment ((x1, y1), (x2, y2)), VerticalLine verticalX) -> 
        if not (x1 === x2) then
            let segmentLine = getSegmentLine x1 y1 x2 y2
            if diffSidesPoints x1 x2 verticalX then
                match segmentLine with 
                | Line (segmentA, segmentB) -> Point (verticalX, segmentA * verticalX + segmentB)
                | _ -> failwith "Error"
            else NoPoint
        else 
            if x1 = verticalX then LineSegment ((x1, y1), (x2, y2)) 
            else NoPoint

    | (LineSegment ((x1, y1), (x2, y2)), LineSegment ((x3, y3), (x4, y4))) -> 
        if not (x1 === x2) && not (x3 === x4) then // Nonvertical LineSegments
            let segment1Line = getSegmentLine x1 y1 x2 y2
            let segment2Line = getSegmentLine x3 y3 x4 y4

            if segment1Line.Equals(segment2Line) then // On the same line
                if diffSidesPoints x1 x2 x3 || diffSidesPoints x1 x2 x4 || diffSidesPoints x3 x4 x1 || diffSidesPoints x3 x4 x2 then
                    // Intersect

                    let sortedXList = List.sort [ x1; x2; x3; x4 ]
                    let (goodX1, goodX2) = ((List.nth sortedXList 1), (List.nth sortedXList 2))
                    let (goodY1, goodY2) = ((if goodX1 = x1 then y1
                                             else if goodX1 = x2 then y2
                                             else if goodX1 = x3 then y3
                                             else y4
                                            ),
                                            (if goodX2 = x1 then y1
                                             else if goodX2 = x2 then y2
                                             else if goodX2 = x3 then y3
                                             else y4
                                            )
                                           )
                    if goodX1 = goodX2 then Point (goodX1, goodY1)
                    else LineSegment ((goodX1, goodY1), (goodX2, goodY2))  
                else NoPoint // Don't intersect

            else
                match (segment1Line, segment2Line) with
                | (Line (a1, _), Line (a2, _)) -> 
                    if a1 === a2 then NoPoint // On parallel lines but not the same
                    else // On nonparallel lines
                        let lineIntersectionPoint = intersect segment1Line segment2Line // Will definitely be a point
                        match lineIntersectionPoint with
                        | Point (interX, interY) -> if diffSidesPoints x1 x2 interX then lineIntersectionPoint else NoPoint 
                                                    // Intersect if line intersection is inside any LineSegment per X Axis
                        | _ -> failwith "Error"      
                | _ -> failwith "Error"

        else if x1 === x2 && x3 === x4 then // Both LineSegments are vertical
            if x2 === x3 then // On the same line
                if diffSidesPoints y1 y2 y3 || diffSidesPoints y1 y2 y4 || diffSidesPoints y3 y4 y1 || diffSidesPoints y3 y4 y2 then
                    // Intersect 

                    let sortedYList = List.sort [ y1; y2; y3; y4 ]

                    if sortedYList.Tail.Head = sortedYList.Tail.Tail.Head then Point (x2, sortedYList.Tail.Head)
                    else LineSegment ((x2, sortedYList.Tail.Head), (x2, sortedYList.Tail.Tail.Head))
                else NoPoint // Don't intersect
            else NoPoint // On the different lines

        else // One LineSegment is vertical, the other is not
            let verticalSegment = (if x1 === x2 then LineSegment ((x1, y1), (x2, y2)) else LineSegment ((x3, y3), (x4, y4)))
            let nonVerticalSegment = (if not (x1 === x2) then LineSegment ((x1, y1), (x2, y2)) else LineSegment ((x3, y3), (x4, y4)))

            match (verticalSegment, nonVerticalSegment) with
            | (LineSegment ((vx, _), (_, _)), LineSegment ((nvx1, nvy1), (nvx2, nvy2))) -> 
                if (diffSidesPoints nvx1 nvx2 vx) then                                                                                                                                                          
                     // Intersect per X axis, 
                     // intersection = intersection between nonvertical LineSegment line and vertical LineSegment

                     let nonVerticalSegmentLine = getSegmentLine nvx1 nvy1 nvx2 nvy2
                     intersect nonVerticalSegmentLine verticalSegment
                 else NoPoint // Don't intersect per X axis
            | _ -> failwith "Error"

    | (Intersect (innerSet1, innerSet2), Intersect (innerSet3, innerSet4)) -> intersect (intersect innerSet1 innerSet2) (intersect innerSet3 innerSet4)
    | (Intersect (innerSet1, innerSet2), _) -> intersect (intersect innerSet1 innerSet2) set2 
    | (_, Intersect (innerSet1, innerSet2)) -> intersect set1 (intersect innerSet1 innerSet2)
