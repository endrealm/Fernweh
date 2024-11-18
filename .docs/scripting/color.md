---
order: 79
---

# Coloring
Colors are set in various places like translation text or as a background color. Each time this will be a string value.

## Literals
We support all [XNA/Monogame colors](http://www.foszor.com/blog/xna-color-chart/) as literals. It is important that to notice that the **values are case sensitive**!
Of course you can also define your own color values.

## RGB / RGBA
You can also use RGB values the syntaxt is the following:
```lua
"rgb(12,112,12)"
```
The values may range from 0 to 255;

You can also use transparency with the following syntax:
```lua
"rgba(12,112,12,.5)"
"rgba(12,112,12,0.5)"
```
Notice that we use `rgba` instead of `rgb`. The last parameter ranges from 0-1!