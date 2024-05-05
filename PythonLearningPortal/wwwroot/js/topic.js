function toggleContent(topicId) {
    var topicContent = document.getElementById(topicId);
    var allTopics = document.querySelectorAll(".topic-content");
    allTopics.forEach(function (topic) {
        if (topic.id !== topicId) {
            topic.style.display = "none";
        }
    });
    if (topicContent.style.display === "block") {
        topicContent.style.display = "none";
    } else {
        topicContent.style.display = "block";
    }
}

function toggleSubContent(subtopicId) {
    var subtopicContent = document.getElementById(subtopicId);
    var allSubtopics = document.querySelectorAll(".subtopic-content");
    allSubtopics.forEach(function (subtopic) {
        if (subtopic.id !== subtopicId) {
            subtopic.style.display = "none";
        }
    });
    if (subtopicContent.style.display === "block") {
        subtopicContent.style.display = "none";
    } else {
        subtopicContent.style.display = "block";
    }
}