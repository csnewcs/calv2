const port = 8080

const fs = require('fs')
const url = require('url')
const express = require('express')
const app = express()
const path = require('path').resolve()

app.use(express.json())
// app.set('view engine', 'ejs')
// app.engine('html', require('ejs').renderFile)
let data = [
]
// let order = 0

var server = app.listen(port, function () {
    try  {
        data = JSON.parse(fs.readFileSync(path + '/data/senddata.json'))
    } catch {
        !fs.existsSync(path + '/data') && fs.mkdirSync(path + '/data')
        data = JSON.parse('[]')
        // fs.writeFile(path + '/data/senddata.json')
    }
    console.log('Server started')
})

app.get('/', function(req, res) {
    res.sendFile(path + '/pages/index.html')
    console.log('서버 접속')
})

app.get('/rank', function (req, res) {
    res.sendFile(path + '/pages/rank.html')
    console.log('순위 조회')
})

app.get('/api/read', function (req, res) {
    console.log(req.ip + ' get')
    res.send(data)
})

app.put('/api/add', function (req, res) {
    console.log(req.body)
    let json = req.body
    let storeJson = JSON.parse('[]')
    let sendJson = JSON.parse('[]')
    try  {
        storeJson = JSON.parse(fs.readFileSync(path + '/data/data.json'))
        sendJson = JSON.parse(fs.readFileSync(path + '/data/senddata.json'))
    } catch {
        !fs.existsSync(path + '/data') && fs.mkdirSync(path + '/data')
    }
    storeJson = json
    fs.writeFile(path + '/data/data.json', JSON.stringify(storeJson), 'utf8', function(error, data) {})
    
    if (json.hideName == 1) { //0: 안가림, 1: 2번째 글자 가림, 2: 전체 가림
        json.name = hideName(json.name)
    }
    else if (json.hideName == 2) {
        json.name = '***'
    }
    sendJson.push(json)
    fs.writeFile(path + '/data/senddata.json', JSON.stringify(sendJson), 'utf8', function(error, data) {})
    data.push(json)
    res.status(200).json({message: "success"})
})

function hideName(name) {
    returnName = ''
    for (i = 0; i < name.length; i++) {
        if (i == 1) {
            returnName += '*'
            continue
        }
        returnName += name[i]
    }
    return returnName
}