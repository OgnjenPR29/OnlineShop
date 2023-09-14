import dayjs from 'dayjs';

export function formatDateTime(dateTime) {
  const date = new Date(dateTime);
  const options = { day: 'numeric', month: 'long', year: 'numeric' };

  return date.toLocaleDateString(date, options);
}

//YYYY-MM-DD
export function getDateString(dateString) {
  const date = new Date(dateString);
  console.log(dateString)
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const day = String(date.getDate()).padStart(2, '0');

  dateString = `${year}-${month}-${day}`;
  
  return dateString;
}

export function getDayjs(value) {
  const date = new dayjs(getDateString(value));

  return date;
}

//YYYY-MM-DDT00:00:00
export function getIsoString(value) {
  const date = new Date(getDateString(value));
  const isoDate = date.toISOString();

  return isoDate;
}
