const express = require('express');
const app = express();
const server = app.listen(5500, ()=>console.log('Running server on http://localhost:5500'));app.use(express.static('public'));
app.set('view engine', 'ejs');
app.get('/', (req, res)=>res.render('Home.ejs'));
app.get('/about', (req, res)=>res.render('About.ejs'));