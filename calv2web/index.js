const port = 8080

const fs = require('fs')
const url = require('url')
const express = require('express')
const app = express()
const path = require('path').resolve()

// app.set('view engine', 'ejs')
// app.engine('html', require('ejs').renderFile)

var server = app.listen(port, function () {
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