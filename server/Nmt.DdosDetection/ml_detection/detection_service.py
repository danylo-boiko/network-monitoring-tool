import pandas
import pickle
import os

from pandas import DataFrame
from pandas.io.parsers import TextFileReader
from sklearn.neural_network import MLPClassifier
from sklearn.model_selection import train_test_split
from sklearn.metrics import classification_report, confusion_matrix
from sklearn.preprocessing import LabelEncoder
from ipaddress import ip_address
from timeit import default_timer as timer


class DetectionService:
    def __init__(self, ml_model_name: str):
        self.current_dir = os.getcwd() + '/ml_detection'
        self.ml_model_name = ml_model_name
        self.ml_model_columns = ['Protocol', 'IP', 'Size']

    def train_model(self) -> None:
        """
        hidden_layer_sizes = array of the hidden layer of the network, (5) = one layer of 5 nodes, (5,5) = 2 layers,
                             both with 5 nodes
        activation = activation function, 'logistic' is equivalent ot the sigmoid activation function
        max_iter = maximum amount of iterations that the model will do
        verbose = whether the model prints the iteration and loss function per iteration
        tol = the decimal place the use wants the loss function to reach
        """
        classifier = MLPClassifier(hidden_layer_sizes=(100, 100), activation='logistic', max_iter=1000, verbose=True,
                                   tol=0.00000001, early_stopping=True, shuffle=True)

        csv = pandas.read_csv(self._get_dataset_file(), delimiter=',')
        data = self._encode_labels(csv)

        x = data[self.ml_model_columns]
        y = data['Target']

        x_train, x_test, y_train, y_test = train_test_split(x.values, y)

        start_time = timer()
        classifier.fit(x_train, y_train)
        time_taken = timer() - start_time

        predictions = classifier.predict(x_test)

        hostile = safe = 0
        for prediction in predictions:
            if prediction == 1:
                hostile += 1
            else:
                safe += 1

        print(f'Number of iterations: {classifier.n_iter_}')
        print(f'Safe packets: {safe}')
        print(f'Hostile packets: {hostile}')
        print(f'Time taken: {time_taken}')
        print("Confusion matrix", "\n", confusion_matrix(y_test, predictions))
        print("Classification report", "\n",  classification_report(y_test, predictions))

        pickle.dump(classifier, open(self._get_model_file(), 'wb'))

    def detect_hostile_ips(self, packets: DataFrame) -> list[int]:
        ips = list(packets['IP'])
        data = self._encode_labels(packets)
        x = data[self.ml_model_columns]

        trained_model = pickle.load(open(self._get_model_file(), 'rb'))
        predictions = trained_model.predict(x.values)

        hostile_ips = set()
        for i in range(len(predictions)):
            if predictions[i] == 1:
                hostile_ips.add(ip_address(ips[i]).__int__())

        return list(hostile_ips)

    def _encode_labels(self, csv: TextFileReader | DataFrame) -> TextFileReader | DataFrame:
        labels_to_encode = list(csv.select_dtypes(include=['category', 'object']))
        encoder = LabelEncoder()

        for label in labels_to_encode:
            csv[label] = encoder.fit_transform(csv[label])

        return csv

    def _get_model_file(self) -> str:
        return f'{self.current_dir}/trained_models/{self.ml_model_name}.sav'

    def _get_dataset_file(self) -> str:
        return f'{self.current_dir}/datasets/{self.ml_model_name}.csv'
