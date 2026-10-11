1. Introduction


Part 2 extends the Municipal Services Application developed in Part 1. It introduces a Local Events and Announcements page that allows users to browse upcoming municipal events, search by keyword and category, filter events by date, view search history, display priority events and receive recommendations based on their search interests.

The application demonstrates Windows Forms development, event handling, collections, data structures and a basic recommendation algorithm.




2. Application Features

2.1 Main Menu

The main menu provides access to Report Issues and Local Events and Announcements. The Service Request Status option remains reserved for a later task.

Selecting Local Events and Announcements opens the Events form. The existing Report Issues functionality from Part 1 is retained.

2.2 Local Events and Announcements

The Events form displays sample municipal events with their dates, names, categories and locations.

The included sample events are:

- Community Clean-Up
- Youth Skills Workshop
- Water Services Update
- Local Arts Festival
- Road Safety Awareness

2.3 Search and Filtering

Users can search for events using keywords and select a category. The date filter allows users to display events scheduled for a particular date. The Show All button clears the current search and restores the complete event list.

2.4 Search History

The application records non-empty search terms in a stack and a queue during the current session. The Show Search History button displays the recorded search terms, with the most recent searches appearing first.

The search history is stored in memory and is reset when the application is restarted.

2.5 Priority Events

The application organises events into priority groups using a sorted dictionary containing queues. The Show Priority Events button displays the events grouped by priority.

2.6 Search-Based Recommendations

The recommendation feature analyses recent search terms and compares them with event names, categories, descriptions and locations.

A scoring algorithm assigns points for matches. Matches in event names and categories receive more points than matches in descriptions and locations. The highest-scoring events are displayed first, with event dates used to order events with equal scores.

This provides users with suggestions related to their recorded search interests.



3. Data Structures Used

List<MunicipalEvent>

Stores the sample municipal events in a collection that can be searched, filtered and displayed.

Stack<string>

Stores recent search terms using last-in, first-out (LIFO) behaviour. This allows the application to access recent searches first.

Queue<string>

Records search terms using first-in, first-out (FIFO) behaviour.

Dictionary<string, List<MunicipalEvent>>

Groups events by category using category names as keys.

SortedDictionary<DateTime, List<MunicipalEvent>>

Organises events by date, keeping date keys in sorted order.

HashSet<string>

Stores unique event categories and prevents duplicate category names.

SortedDictionary<int, Queue<MunicipalEvent>>

Group events by priority. The sorted dictionary orders priority keys, while each queue stores events assigned to that priority.

These collections demonstrate different ways of organising, grouping and retrieving event-related information.



4. How to Compile and Run the Application

Requirements:

- Windows operating system.
- Visual Studio with Windows Forms and .NET Framework development support.
- The supplied Municipal Services Application source code and solution file.

Instructions:

1. Extract the submitted project folder if it is compressed.
2. Open "MunicipalServicesApp.sln" in Visual Studio.
3. Allow Visual Studio to load the solution.
4. Select Build → Rebuild Solution.
5. Confirm that the solution builds without errors.
6. Select Debug → Start Without Debugging, or press Ctrl + F5.
7. The Municipal Services main menu should appear.



5. How to Use the Application

1. Open the application and select Local Events and Announcements.
2. Browse the five sample events in the Available Events list.
3. Enter a keyword, such as "Community" or "Youth", and click Search.
4. Select a category to filter the results.
5. To filter by date, tick Filter by date, choose a date and click Search.
6. Click Show All to restore the complete event list.
7. Click Show Search History to display recorded searches.
8. Click Show Priority Events to display events organised by priority.
9. Review Recommended Events to see suggestions and their match scores.
10. Click Back to Main Menu to return to the main menu.



6. Conclusion

Part 2 extends the Municipal Services Application by introducing a functional events and announcements page. It demonstrates searching, filtering, event organisation, search history, priority grouping and recommendations based on recent user searches.

The implementation provides a practical demonstration of C# Windows Forms and data structures relevant to managing municipal event information.
