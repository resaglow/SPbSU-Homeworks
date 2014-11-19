module emailDetector

open System.Text.RegularExpressions

let checkEmail email = 
    let username = "[a-zA-Z_]((\.)?[\w-_]){0,30}"
    let domains = "([\w]+\.)+"
    let globalDomain = "([a-zA-Z]{2,3}|aero|asia|coop|info|jobs|mobi|museum|name|travel)"

    let myPattern = "^" + username + "@" + domains + globalDomain + "$"

    Regex.IsMatch(email, myPattern)