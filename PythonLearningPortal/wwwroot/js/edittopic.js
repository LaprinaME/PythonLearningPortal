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

document.addEventListener('DOMContentLoaded', (event) => {
    // Находим все элементы с классом "description" и добавляем обработчик события для их редактирования
    const descriptions = document.querySelectorAll('.description');
    descriptions.forEach(description => {
        description.addEventListener('click', () => {
            // Создаем поле ввода textarea для редактирования
            const textarea = document.createElement('textarea');
            textarea.className = 'description-edit';
            textarea.value = description.innerText;
            description.parentElement.replaceChild(textarea, description);

            // Добавляем кнопки "Сохранить" и "Отмена" для завершения редактирования
            const saveButton = document.createElement('button');
            saveButton.innerText = 'Сохранить';
            saveButton.className = 'save-button';
            const cancelButton = document.createElement('button');
            cancelButton.innerText = 'Отмена';
            cancelButton.className = 'cancel-button';
            textarea.insertAdjacentElement('afterend', saveButton);
            textarea.insertAdjacentElement('afterend', cancelButton);

            // Добавляем обработчики событий для кнопок
            saveButton.addEventListener('click', () => {
                // Сохраняем изменения, заменяя поле ввода textarea на отредактированный текст
                description.innerText = textarea.value;
                textarea.parentElement.removeChild(textarea);
                saveButton.parentElement.removeChild(saveButton);
                cancelButton.parentElement.removeChild(cancelButton);
            });

            cancelButton.addEventListener('click', () => {
                // Отменяем редактирование и возвращаемся к изначальному тексту
                textarea.parentElement.replaceChild(description, textarea);
                saveButton.parentElement.removeChild(saveButton);
                cancelButton.parentElement.removeChild(cancelButton);
            });
        });
    });
});