# TextToKKT v 5.5.2
> PCC: 00077E4991714B1C


Утилита сервис-инженера ККТ (контрольно-кассовая техника, 54-ФЗ)


#

## Описание

Инструмент позволяет:
- просматривать инструкции по работе с ККТ;
- просматривать расшифровки кодов ошибок ККТ;
- определять срок жизни ФН в соответствии параметрами пользователя, о которых он зависит;
- определять соответствие модели ФН ФФД 1.2;
- определять корректность регистрационного номера ККТ и генерировать его при необходимости;
- определять соответствие модели ККТ версиям ФФД 1.1 и 1.2;
- получать настройки оператора фискальных данных (ОФД) по его ИНН или названию;
- получать описания TLV-тегов для ФФД 1.2;
- получать описания команд нижнего уровня для фискальных регистраторов;
- вручную (без использования внешней клавиатуры) программировать текстовые данные в ККТ, имеющих только цифровую клавиатуру;
- автоматически определять модели ККТ и фискальных накопителей (ФН) по их заводским номерам, а также регионы РФ по кодам ИНН;
- *проверять товарные штрих-коды и извлекать доступные сведения (в разработке)*.

Версия для Windows теперь имеет дополнительный модуль для работы с данными фискальных накопителей (см. далее).

&nbsp;



## Модуль FNReader

Компонент FNReader для Windows предназначен для чтения и обработки фискальных данных (ФД) из фискального накопителя (ФН).

На данный момент в модуле доступны следующие функции:

1. Чтение данных из ФН с помощью физического или виртуального COM-порта и аппаратного адаптера.

2. Формирование выгрузки архива ФН в формате .fnc **в полном соответствии с пунктами 45 – 58 Приложения 2 к приказу ФНС России
«Форматы фискальных документов, обязательные к использованию» для версии ФФД 1.05**.

3. Обработка фискальных данных:
- Получение полного состояния и всех регистрационных данных ФН;
- Получение отдельного документа ФН;
- Получение полного отчёта по архиву ФН;
- Получение контрольной ленты за отдельную смену;
- Получение полного фискального (посменного) отчёта;
- Получение посменного отчёта по диапазону дат.

4. Сохранение архива ФН в файл в формате:
- Двоичных данных (доступен для последующего открытия и обработки в программе); **это – внутренний формат программы (.fsd),
  который не может быть использован при перерегистрации или снятии ККТ с учёта в личном кабинете ФНС**;
- Текстовой контрольной ленты;
- Табличных данных (доступен для обработки в Microsoft Office Excel).

5. Выполнение обмена с ОФД с параметрами подключения, получаемыми автоматически по данным последней регистрации / перерегистрации.

6. Сброс МГМ (для технических целей).

7. Закрытие смены и архива ФН текущей датой или датой последнего документа ФН (когда это нереализуемо на ККТ).

8. Чтение выгрузок архивов ФН в форматах .fnc и .fsd.

#

Чтение может быть выполнено тремя способами:

- *Полное чтение архива*. По его завершении становятся доступными все функции обработки данных.

- *Прямое чтение ФН*. В этом случае доступны функции чтения статуса ФН, отдельного документа и контрольной
ленты за смену (при условии, если функция поддерживается накопителем).

- *Формирование выгрузки с последующей её загрузкой*. Функция позволяет самостоятельно формировать файл
выгрузки ФН, который (аналогично созданным в других программах) может быть открыт в FNReader.

Независимо от варианта и настроек чтения текущее состояние ФН запрашивается в максимально полном виде.

#

Детализация чтения также может быть:

- *Полной*. При этом из ФН считываются все основные текстовые (TLV) поля и квитанции подтверждения ОФД. Может занимать продолжительное
время. Наиболее старые версии ФН не поддерживают полную выгрузку.

- *Краткой*. При этом считываются только суммовые счётчики, временны́е метки, номера, фискальные признаки документов и состояния
отправки ОФД. Выполняется быстрее полного считывания (примерно в 2 раза).

&nbsp;



## Требования

- Актуальная Windows-версия утилиты доступна в [релизах проекта](https://github.com/adslbarxatov/TextToKKT/releases);
для неё потребуется ОС Windows XP или новее, Framework 4.0 или новее;
- Android-версия доступна на [Google play](https://play.google.com/store/apps/details?id=com.RD_AAOW.TextToKKT);
для неё потребуется ОС Android 5.0 или новее;
- Android-версия доступна в [специальном релизе](https://github.com/adslbarxatov/TextToKKT/releases/tag/v5.1) для Android 4.x
или устройств без Play market;
- Android-версия доступна также на платформе AppGallery

&nbsp;



## Требования к оборудованию для работы модуля FNReader

Работа программы протестирована на всех моделях ФН из реестра ФНС.
При соблюдении производителями существующего аппаратного протокола чтения данных работа с будущими моделями ФН также будет возможна.
**Поддержка версий ФФД 1.1 и более новых на данный момент не гарантируется**.

Работа программы возможна при наличии следующих аппаратных адаптеров:

- COM-UART переходник для подключения ФН к разъёму COM (RS-232) компьютера;

- USB-VCOM-UART переходник для подключения к разъёму USB компьютера. В этом случае потребуется установить драйвера устройства;
инструкции и ссылки для их установки предоставляются производителями таких переходников.

**Обращаем внимание, что срок действия каждого релиза модуля ограничен с целью устранения устаревающих версий
программы и обеспечения постоянного соответствия актуальным изменениям в ФФД. Срок указан в заголовке окна компонента.
По его истечении останутся доступными функции анализа ранее считанных данных. По истечению срока для работы с новыми данными
необходимо получить новый экземпляр компонента**

#

- Язык интерфейса и руководства пользователя: ru_ru.
- Список поддерживаемых TLV-тегов можно посмотреть [здесь](https://github.com/adslbarxatov/TextToKKT/blob/master/src/FNReader/FNEnums.h).
- По вопросам предоставления доступа к приложению просим обращаться на [электронную почту](mailto://adslbarxatov@mail.ru)

&nbsp;



## [Политика разработки и EULA](https://adslbarxatov.github.io/ADP)

Данная Политика (ADP), её положения, заключение, EULA и способы применения
описывают общие правила, которым мы следуем во всех наших процессах разработки, вышедших в релиз приложениях
и реализованных идеях.
***Обязательна к ознакомлению для всех участников и пользователей перед использованием любого из продуктов лаборатории.
Загружая их, вы соглашаетесь и принимаете эту Политику!***
