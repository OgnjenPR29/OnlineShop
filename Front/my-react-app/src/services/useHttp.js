import { useCallback, useState } from 'react';
import { getRawToken } from '../helper/tokenUtils';

const useHttp = (resolve) => {
  const [data, setData] = useState(null);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const [statusCode, setStatusCode] = useState(null);

  const prepare = () => {
    setIsLoading(true);
    setError(null);
    setStatusCode(null);
    setData(null);
  };

  const processResponse = (response) => {
    setStatusCode(response.status);
    // Action if its JSON response
    if (response.headers.get('content-type')?.includes('application/json')) {
      if (!response.ok) {
        return response.json().then((data) => {
          const errorMessage = data.message || data.title || '';

          throw new Error(errorMessage);
        });
      }
      return response.json();
    }
    // Action if its TEXT response
    else if (response.headers.get('content-type')?.includes('text/plain')) {
      if (!response.ok) {
        return response.text().then((errorMessage) => {
          throw new Error(errorMessage);
        });
      }
    }
    // Action for neither
    else {
      if (!response.ok) {
        const errorMessage = response?.title || '';
        throw new Error(errorMessage);
      }
    }
  };

  const getRequest = useCallback(
    (url) => {
      prepare();

      console.log(getRawToken())

      fetch(url, {
        method: 'GET',
        mode: 'cors',
        headers: {
          'Content-Type': 'application/json',
          Authorization: 'Bearer ' + getRawToken(),
        },
      })
        .then((response) => {
          return processResponse(response);
        })
        .then((data) => {
          setData(data);
        })
        .catch((error) => {
          setError('Error doing GET request! ' + error);
        })
        .finally(() => {
          setIsLoading(false);
          resolve && resolve();
        });
    },
    [resolve]
  );

  const postRequest = useCallback(
    (url, data) => {
      prepare();

      fetch(url, {
        method: 'post',
        body: JSON.stringify(data),
        mode: 'cors',
        headers: {
          'Content-Type': 'application/json',
          Authorization: 'Bearer ' + getRawToken(),
        },
      })
        .then((response) => {
          return processResponse(response);
        })
        .then((data) => {
          setData(data);
        })
        .catch((error) => {
          setError('Error doing POST request! ' + error.message);
        })
        .finally(() => {
          setIsLoading(false);
          resolve && resolve();
        });
    },
    [resolve]
  );

  const putRequest = useCallback(
    (url, data) => {
      prepare();

      fetch(url, {
        method: 'put',
        body: JSON.stringify(data),
        mode: 'cors',
        headers: {
          'Content-Type': 'application/json',
          Authorization: 'Bearer ' + getRawToken(),
        },
      })
        .then((response) => {
          return processResponse(response);
        })
        .then((data) => {
          setData(data);
        })
        .catch((error) => {
          setError('Error doing PUT request! ' + error.message);
        })
        .finally(() => {
          setIsLoading(false);
          resolve && resolve();
        });
    },
    [resolve]
  );

  const putRequestFormData = useCallback(
    (url, data) => {
      prepare();

      const formData = new FormData();
      for (const key in data) {
        formData.append(key, data[key]);
      }

      fetch(url, {
        method: 'put',
        body: formData,
        mode: 'cors',
        headers: {
          Authorization: 'Bearer ' + getRawToken(),
        },
      })
        .then((response) => {
          return processResponse(response);
        })
        .then((data) => {
          setData(data);
        })
        .catch((error) => {
          setError('Error doing PUT request! ' + error.message);
        })
        .finally(() => {
          setIsLoading(false);
          resolve && resolve();
        });
    },
    [resolve]
  );

  const postRequestFormData = useCallback(

    (url, data) => {
      prepare();

      const formData = new FormData();
      for (const key in data) {
        formData.append(key, data[key]);
      }

      fetch(url, {
        method: 'post',
        body: formData,
        mode: 'cors',
        headers: {
          Authorization: 'Bearer ' + getRawToken(),
        },
      })
        .then((response) => {
          return processResponse(response);
        })
        .then((data) => {
          setData(data);
        })
        .catch((error) => {
          setError('Error doing POST request! ' + error.message);
        })
        .finally(() => {
          setIsLoading(false);
          resolve && resolve();
        });
    },
    [resolve]
  );

  const deleteRequest = useCallback(
    (url) => {
      prepare();

      fetch(url, {
        method: 'delete',
        mode: 'cors',
        headers: {
          'Content-Type': 'application/json',
          Authorization: 'Bearer ' + getRawToken(),
        },
      })
        .then((response) => {
          return processResponse(response);
        })
        .then((data) => {
          setData(data);
        })
        .catch((error) => {
          setError('Error doing DELETE request! ' + error.message);
        })
        .finally(() => {
          setIsLoading(false);
          resolve && resolve();
        });
    },
    [resolve]
  );

  const resetHttp = () => {
    setData(null);
    setError(null);
    setIsLoading(false);
    setStatusCode(null);
  };

  return {
    data,
    isLoading,
    error,
    statusCode,
    getRequest,
    postRequest,
    postRequestFormData,
    putRequest,
    putRequestFormData,
    deleteRequest,
    resetHttp,
  };
};

export default useHttp;
