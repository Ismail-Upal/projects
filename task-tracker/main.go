package main

import (
	"bufio"
	"encoding/json"
	"fmt"
	"os"
	"strings"
	"time"
)

type Task struct {
	Id          int       `json:"id"`
	Description string    `json:"description"`
	Status      Status    `json:"status"`
	CreatedAt   time.Time `json:"createdAt"`
	UpdatedAt   time.Time `json:"updatedAt"`
}

type Status struct {
	Todo       bool `json:"todo"`
	InProgress bool `json:"in_progress"`
	Done       bool `json:"done"`
}

var tasks []Task

// ======================== CORE LOGIC =========================

func AddTask() {
	reader := bufio.NewReader(os.Stdin)
	input, _ := reader.ReadString('\n')
	s := strings.TrimSpace(input)
	s = strings.Trim(s, "\"") // handle quotes
	id := len(tasks) + 1

	tasks = append(tasks, Task{
		Id:          id,
		Description: s,
		Status:      Status{Todo: true},
		CreatedAt:   time.Now(),
		UpdatedAt:   time.Now(),
	})

	fmt.Printf("Task added successfully (ID: %v)\n", id)
	SaveTasks()
}

func ShowList(stat string) {
	for _, t := range tasks {
		show := false
		switch stat {
		case "":
			show = true
		case "todo":
			show = t.Status.Todo
		case "in-progress":
			show = t.Status.InProgress
		case "done":
			show = t.Status.Done
		}

		if show {
			fmt.Printf("%d: %s | Created: %s | Updated: %s\n",
				t.Id, t.Description,
				t.CreatedAt.Format("2006-01-02 15:04"),
				t.UpdatedAt.Format("2006-01-02 15:04"))
		}
	}
}

func UpdateTask() {
	var id int
	fmt.Scan(&id)

	if id < 1 || id > len(tasks) {
		fmt.Println("Invalid ID")
		return
	}

	reader := bufio.NewReader(os.Stdin)
	input, _ := reader.ReadString('\n')
	s := strings.TrimSpace(input)
	s = strings.Trim(s, "\"")

	tasks[id-1].Description = s
	tasks[id-1].UpdatedAt = time.Now()

	fmt.Println("Task updated successfully")
	SaveTasks()
}

func DeleteTask() {
	var id int
	fmt.Scan(&id)

	if id < 1 || id > len(tasks) {
		fmt.Println("Invalid ID")
		return
	}

	id-- // index
	tasks = append(tasks[:id], tasks[id+1:]...)
	for i := range tasks {
		tasks[i].Id = i + 1
	}

	fmt.Println("Task deleted successfully")
	SaveTasks()
}

func MarkInProgress() {
	var id int
	fmt.Scan(&id)
	if id < 1 || id > len(tasks) {
		fmt.Println("Invalid ID")
		return
	}

	id--
	tasks[id].Status = Status{Todo: false, InProgress: true, Done: false}
	tasks[id].UpdatedAt = time.Now()

	fmt.Println("Marked as in-progress")
	SaveTasks()
}

func MarkDone() {
	var id int
	fmt.Scan(&id)
	if id < 1 || id > len(tasks) {
		fmt.Println("Invalid ID")
		return
	}

	id--
	tasks[id].Status = Status{Todo: false, InProgress: false, Done: true}
	tasks[id].UpdatedAt = time.Now()

	fmt.Println("Marked as done")
	SaveTasks()
}

// ======================== FILE HANDLING =========================

func SaveTasks() {
	data, err := json.MarshalIndent(tasks, "", "  ")
	if err != nil {
		fmt.Println("Error converting to JSON:", err)
		return
	}

	err = os.WriteFile("tasks.json", data, 0644)
	if err != nil {
		fmt.Println("Error writing file:", err)
	}
}

func LoadTasks() {
	data, err := os.ReadFile("tasks.json")
	if err != nil {
		if os.IsNotExist(err) {
			tasks = []Task{}
			return
		}
		fmt.Println("Error reading file:", err)
		return
	}

	err = json.Unmarshal(data, &tasks)
	if err != nil {
		fmt.Println("Error parsing JSON:", err)
	}
}

// ======================== MAIN LOOP =========================

func main() {
	fmt.Println("\nWelcome to Task Tracker CLI <-.->\n")

	LoadTasks()

	for {
		var command string
		fmt.Print("> ")
		fmt.Scan(&command)

		if command == "task-cli" {
			var op string
			fmt.Scan(&op)

			switch op {
			case "add":
				AddTask()
			case "update":
				UpdateTask()
			case "delete":
				DeleteTask()
			case "mark-in-progress":
				MarkInProgress()
			case "mark-done":
				MarkDone()
			case "list":
				var stat string
				fmt.Scanln(&stat)
				stat = strings.TrimSpace(stat)
				ShowList(stat)
			default:
				fmt.Println("Unknown operation!")
			}

		} else if command == "end" {
			fmt.Println("\nGoodbye (-_-) ")
			break
		} else {
			fmt.Println("Invalid command!")
		}
		fmt.Println()
	}
}
