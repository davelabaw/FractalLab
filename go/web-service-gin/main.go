package main

import (
	"net/http"

	"github.com/gin-gonic/gin"
)

type thing struct {
	ID    string `json:"id"`
	Title string `json:"title"`
}

var things = []thing{
	{ID: "1", Title: "One"},
	{ID: "2", Title: "Two"},
	{ID: "3", Title: "Three"},
}

func getThings(c *gin.Context) {
	c.IndentedJSON(http.StatusOK, things)
}

func main() {
	router := gin.Default()
	router.GET("/things", getThings)

	router.Run("localhost:8080")
}
