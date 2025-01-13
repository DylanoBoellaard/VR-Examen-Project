using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class OrderBooks : MonoBehaviour
{
    // List of XRSocketInteractors (sockets)
    public List<XRSocketInteractor> sockets = new List<XRSocketInteractor>();

    // List of books (GameObjects)
    public List<GameObject> books = new List<GameObject>();

    // Store the last positions of books
    private Vector3[] lastPositions;

    // Track if the position has been logged
    private bool[] positionLogged;

    // Track if victory message has been displayed
    private bool victoryMessageDisplayed = false;

    // Reference to the LightController
    public LightController lightController;

    void Start()
    {
        // Initialize the last positions of the books
        lastPositions = new Vector3[books.Count];

        // Initialize the tracking array
        positionLogged = new bool[books.Count];

        // For loop to go through all of the book and log their positions
        for (int i = 0; i < books.Count; i++)
        {
            // Store initial positions of the books
            lastPositions[i] = books[i].transform.position;

            // Initialize all to false
            positionLogged[i] = false;
        }
    }

    void Update()
    {
        // always sets allAllign to true on every update and sets it to false if it does not meet the criteria. this way it can check every update if its all on the correct position yet.
        bool allAligned = true;

        // FOR TESTING ONLY - Checks if books do not have the same amount as the sockets (not desired)
        if (books.Count != sockets.Count)
        {
            // Log a warning to devs of a mismatched count for the books & sockets
            Debug.LogWarning("Mismatch between number of books and sockets!");
        }

        // Loop through all the sockets
        for (int i = 0; i < sockets.Count; i++)
        {
            // Ensure we don't go out of bounds
            if (i < books.Count)
            {
                // Only check position if the book has moved
                if (books[i].transform.position != lastPositions[i])  // Compare current position with the last stored position
                {
                    // Update the last known position of the books
                    lastPositions[i] = books[i].transform.position;

                    // Compare the positions of a socket to the position of a book to see if they are the same (E.G. Book_1 = Socket_1)
                    if (ArePositionsEqual(sockets[i].transform.position, books[i].transform.position))
                    {
                        // If Position is not logged (yet)...
                        if (!positionLogged[i])
                        {
                            // Then log name of socket & name of book
                            Debug.Log("Socket " + (i + 1) + " and Book " + (i + 1) + " are at the same position.");

                            // Mark as logged
                            positionLogged[i] = true;
                        }
                    }
                    // If positions of socket & book is not correct...
                    else
                    {
                        // Set variable to false since they are no longer aligned
                        positionLogged[i] = false;
                    }
                }

                // Check alignment status for the current socket & book
                if (!ArePositionsEqual(sockets[i].transform.position, books[i].transform.position))
                {
                    // If one pair is not aligned, set flag to false
                    allAligned = false;
                }
            }
        }

        // Check victory condition (If all books & sockets are aligned & victory message has not been displayed yet)
        if (allAligned && !victoryMessageDisplayed)
        {
            // Log victory message
            Debug.Log("Victory! All books and sockets are aligned.");

            // Notify the LightController to turn on light (if it exists)
            if (lightController != null)
            {
                lightController.TurnOnLight();
            }

            // Prevent repeated spamming of victory messages
            victoryMessageDisplayed = true;
        }
        // Else if all sockets and books are not aligned properly
        else if (!allAligned)
        {
            // Reset victory message if alignment is broken
            victoryMessageDisplayed = false;
        }
    }

    // Helper function to check if two positions are close enough
    bool ArePositionsEqual(Vector3 pos1, Vector3 pos2, float tolerance = 0.2f)
    {
        // Return final positions of socket & book
        return Vector3.Distance(pos1, pos2) < tolerance;
    }
}
